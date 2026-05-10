using System.Text.Json;
using ZapRu.Data;
using ZapRu.Entities;
using ZapRu.DTOs.NHTSA;
using Microsoft.EntityFrameworkCore;

namespace ZapRu.Services
{
    public class CarImportService
    {
        private readonly HttpClient _httpClient;
        private readonly ZapRuDbContext _context;

        public CarImportService(HttpClient httpClient, ZapRuDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        
        public async Task ImportPopularCarsAsync()
        {
            
            var popularMakes = new List<string>
            {
                "Toyota",
                "BMW",
                "Audi",
                "Mercedes-Benz",
                "Honda",
                "Ford",
                "Tesla",
                "Volkswagen",
                "Hyundai",
                "Kia",
                "Nissan",
                "Lexus",
                "Mazda",
                "Chevrolet",
                "Porsche"
            };

            
            var makesJson = await _httpClient.GetStringAsync(
                "https://vpic.nhtsa.dot.gov/api/vehicles/getallmakes?format=json");

            
            var makesResponse = JsonSerializer.Deserialize<MakeResponse>(makesJson);

            
            if (makesResponse == null || makesResponse.Results == null)
            {
                return;
            }

            
            foreach (var make in makesResponse.Results)
            {

                var matchedMake = popularMakes.FirstOrDefault(m => m.Equals(make.Make_Name, StringComparison.OrdinalIgnoreCase));

                if (matchedMake == null) continue;

                await ImportModelsForMakeAsync(matchedMake);
            }
        }

        
        private async Task ImportModelsForMakeAsync(string makeName)
        {
            
            var url =
                $"https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMake/{makeName}?format=json";

            
            var modelsJson = await _httpClient.GetStringAsync(url);

            
            var modelResponse =
                JsonSerializer.Deserialize<ModelResponse>(modelsJson);

            
            if (modelResponse == null || modelResponse.Results == null)
            {
                return;
            }

            
            foreach (var item in modelResponse.Results)
            {
                
                if (string.IsNullOrWhiteSpace(item.Model_Name))
                {
                    continue;
                }

                
                var modelName = item.Model_Name.Trim();

                
                if (modelName.Length < 2)
                {
                    continue;
                }

                
                bool exists = await _context.CarModels.AnyAsync(c =>
                    c.Brand == makeName &&
                    c.Model == modelName);

                
                if (exists)
                {
                    continue;
                }

                
                var carModel = new CarModel
                {
                    Brand = makeName,
                    Model = modelName
                };

                
                _context.CarModels.Add(carModel);

                Console.WriteLine($"Добавлена модель: {makeName} {modelName}");
            }

            
            await _context.SaveChangesAsync();
        }
    }
}
