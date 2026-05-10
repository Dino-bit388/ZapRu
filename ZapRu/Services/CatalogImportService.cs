using System.Text.Json;
using ZapRu.Data;
using ZapRu.Entities;
using Microsoft.EntityFrameworkCore;

namespace ZapRu.Services
{
    public class CatalogImportService
    {
        private readonly ZapRuDbContext _context;

        public CatalogImportService(ZapRuDbContext context)
        {
            _context = context;
        }

        public async Task ImportPartsAsync(string jsonFilePath)
        {
            
            var json = await File.ReadAllTextAsync(jsonFilePath);

            
            var parts = JsonSerializer.Deserialize<List<PartsCatalog>>(json);

            if (parts == null || parts.Count == 0) return;

            foreach (var part in parts)
            {
                bool exists = await _context.PartsCatalogs
                    .AnyAsync(p => p.Name == part.Name && p.Category == part.Category);

                if (!exists)
                {
                    _context.PartsCatalogs.Add(part);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}