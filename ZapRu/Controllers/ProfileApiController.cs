using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZapRu.Data;
using ZapRu.Entities;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/profile")]
[ApiController]
public class ProfileApiController : ControllerBase
{
    private readonly ZapRuDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProfileApiController(ZapRuDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();
        return Ok(new
        {
            first_name = user.FirstName,
            last_name = user.LastName,
            email = user.Email,
            phone = user.Phone,
            middle_name = user.MiddleName
        });
    }


    [HttpGet("advertisements")]
    public async Task<IActionResult> GetMyAdvertisements()
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var cars = await _context.Cars
            .Include(c => c.CarModel)
            .Include(c => c.Parts)
            .ThenInclude(p => p.PartsCatalog)
            .Include(c => c.Photos)
            .Where(c => c.SellerID == userId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new
            {
                c.CarID,
                c.CarModelID,
                c.CarModel.Brand,
                c.CarModel.Model,
                c.ManufactureYear,
                c.Mileage,
                c.Condition,
                c.Region,
                c.Status,
                c.CreatedAt,
                Parts = c.Parts.Select(p => new
                {
                    p.PartID,
                    p.PartCatID,
                    p.PartsCatalog.Name,
                    p.Price,
                    p.Condition,
                    p.IsSold,
                    p.ViewsCount,
                    BidsCount = p.Bids.Count(b => b.Status == "pending")
                }),
                Photos = c.Photos.Select(p => p.FilePath)
            })
            .ToListAsync();
        return Ok(cars);
    }

    
    [HttpPost("car")]
    public async Task<IActionResult> CreateCar([FromBody] CreateCarRequest request)
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        if (!await _context.CarModels.AnyAsync(m => m.CarModelID == request.CarModelID))
            return BadRequest(new { message = "Выбранная модель автомобиля не существует" });
        
        var car = new Car
        {
            SellerID = userId,
            CarModelID = request.CarModelID,
            ManufactureYear = request.Year,
            Mileage = request.Mileage,
            Condition = request.Condition,
            Region = request.Region,
            Description = request.Description,
            Status = "active",
            CreatedAt = DateTime.UtcNow
        };
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

       
        foreach (var partReq in request.Parts)
        {
            var part = new Part
            {
                CarID = car.CarID,
                PartCatID = partReq.PartCatID,
                Price = partReq.Price,
                Condition = partReq.Condition,
                IsSold = false,
                ViewsCount = 0
            };
            _context.Parts.Add(part);
        }
        await _context.SaveChangesAsync();
        return Ok(new { carID = car.CarID });
    }

 
    [HttpPut("car/{id}")]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarRequest request)
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarID == id && c.SellerID == userId);
        if (car == null) return NotFound();
    
        car.Mileage = request.Mileage;
        car.Condition = request.Condition;
        car.Region = request.Region;
        car.Description = request.Description;
        car.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(new { carID = id, message = "Объявление обновлено" });
    }


    [HttpDelete("car/{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var car = await _context.Cars
            .Include(c => c.Parts)
            .Include(c => c.Photos)
            .FirstOrDefaultAsync(c => c.CarID == id && c.SellerID == userId);

        if (car == null) return NotFound();
        bool hasActiveBids = await _context.Bids
        .AnyAsync(b => car.Parts.Select(p => p.PartID).Contains(b.PartID) && b.Status == "pending");

        if (hasActiveBids)
            return BadRequest(new { message = "Нельзя удалить объявление с активными заявками" });
       

        var partIds = car.Parts.Select(p => p.PartID).ToList();
        var bids = await _context.Bids
       .Where(b => partIds.Contains(b.PartID))
       .ToListAsync();
        _context.Bids.RemoveRange(bids);


        foreach (var photo in car.Photos)
        {
            var filePath = Path.Combine(_env.WebRootPath, photo.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
        }   
        _context.Parts.RemoveRange(car.Parts);
        _context.Photos.RemoveRange(car.Photos);
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return Ok();
    }

  
    [HttpPost("car/{carId}/photos")]
    public async Task<IActionResult> UploadCarPhotos(int carId, List<IFormFile> files)
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var car = await _context.Cars.Include(c => c.Photos).FirstOrDefaultAsync(c => c.CarID == carId && c.SellerID == userId);
        if (car == null) return NotFound();

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "cars", carId.ToString());
        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

        var uploadedPaths = new List<string>();
        foreach (var file in files)
        {
            if (file.Length == 0) continue;
            var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var relativePath = $"/uploads/cars/{carId}/{uniqueName}";
            var photo = new Photo
            {
                EntityType = "car",
                EntityID = carId,
                FilePath = relativePath,
                Uploaded_At = DateTime.UtcNow
            };
            car.Photos.Add(photo);
            uploadedPaths.Add(relativePath);
        }
        await _context.SaveChangesAsync();
        return Ok(uploadedPaths);
    }

    [HttpGet("bids/received")]
    public async Task<IActionResult> GetReceivedBids()
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var bids = await _context.Bids
            .Include(b => b.Part).ThenInclude(p => p.Car).ThenInclude(c => c.CarModel)
            .Include(b => b.Part).ThenInclude(p => p.PartsCatalog)
            .Include(b => b.Buyer)
            .Where(b => b.SellerID == userId && b.Status == "pending")
            .Select(b => new
            {
                b.BidID,
                b.OfferedPrice,
                b.Comment,
                b.CreatedAt,
                PartName = b.Part.PartsCatalog.Name,
                CarName = b.Part.Car.CarModel.Brand + " " + b.Part.Car.CarModel.Model,
                BuyerName = b.Buyer.FirstName + " " + b.Buyer.LastName,
                BuyerPhone = b.Buyer.Phone,
                BuyerEmail = b.Buyer.Email
            })
            .ToListAsync();
        return Ok(bids);
    }


    [HttpPost("bid/{bidId}/accept")]
    public async Task<IActionResult> AcceptBid(int bidId)
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var bid = await _context.Bids
            .Include(b => b.Part)
            .ThenInclude(p => p.Car)
            .FirstOrDefaultAsync(b => b.BidID == bidId && b.SellerID == userId && b.Status == "pending");
        if (bid == null) return NotFound();

        bid.Status = "accepted";
        bid.Part.IsSold = true;
        bid.Part.SoldAt = DateTime.UtcNow;

        bid.Part.Car.Status = "in_deal";

        var otherBids = await _context.Bids
            .Where(b => b.PartID == bid.PartID && b.BidID != bidId && b.Status == "pending")
            .ToListAsync();
        otherBids.ForEach(b => b.Status = "declined");

        var transaction = new Transaction
        {
            PartID = bid.PartID,
            BuyerID = bid.BuyerID,
            SellerID = userId,
            AgreedPrice = bid.OfferedPrice,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarID == bid.Part.CarID);
        if (car != null)
        {
            car.Status = "in_deal"; 
        }
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("bid/{bidId}/reject")]
    public async Task<IActionResult> RejectBid(int bidId)
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);

        var bid = await _context.Bids
            .Include(b => b.Part)
            .FirstOrDefaultAsync(b => b.BidID == bidId && b.SellerID == userId && b.Status == "pending");

        if (bid == null) return NotFound();


        bid.Status = "declined";


        var notification = new Notification
        {
            UserID = bid.BuyerID,
            FromUserID = userId,
            PartID = bid.PartID,
            Type = "bid_declined",
            Message = $"Ваше предложение {bid.OfferedPrice}₽ было отклонено",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();
        return Ok(new { message = "Заявка отклонена" });
    }

    [HttpGet("purchases")]
    public async Task<IActionResult> GetMyPurchases()
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var purchases = await _context.Transactions
            .Include(t => t.Part).ThenInclude(p => p.PartsCatalog)
            .Include(t => t.Part).ThenInclude(p => p.Car).ThenInclude(c => c.CarModel)
            .Include(t => t.Seller)
            .Where(t => t.BuyerID == userId)
            .Select(t => new
            {
                t.TransactionID,
                t.AgreedPrice,
                t.Status,
                t.CreatedAt,
                PartName = t.Part.PartsCatalog.Name,
                CarName = t.Part.Car.CarModel.Brand + " " + t.Part.Car.CarModel.Model,
                SellerName = t.Seller.FirstName + " " + t.Seller.LastName,
                SellerPhone = t.Seller.Phone,
                SellerEmail = t.Seller.Email
            })
            .ToListAsync();
        return Ok(purchases);
    }

    [HttpPost("{partId}/bid")]
    public async Task<IActionResult> CreateBid(int partId, [FromBody] CreateBidRequest request)
    {
        var buyerId = int.Parse(User.FindFirst("UserID").Value);
        var part = await _context.Parts
            .Include(p => p.Car)
            .Include(p => p.PartsCatalog)
            .FirstOrDefaultAsync(p => p.PartID == partId);
        if (part == null) return NotFound();
        if (part.IsSold) return BadRequest(new { message = "Деталь уже продана" });
        var sellerId = part.Car.SellerID;
        if (sellerId == buyerId) return BadRequest(new { message = "Нельзя откликнуться на свою деталь" });

        var bid = new Bid
        {
            PartID = partId,
            BuyerID = buyerId,
            SellerID = sellerId,
            OfferedPrice = request.Price ?? part.Price ?? 0,
            Comment = request.Comment,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };
        _context.Bids.Add(bid);

        var notification = new Notification
        {
            UserID = sellerId,
            FromUserID = buyerId,
            PartID = partId,
            Type = "bid",
            Message = $"Предложение {bid.OfferedPrice}₽ на деталь {part.PartsCatalog.Name}",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Отклик отправлен" });
    }

    public class CreateBidRequest
    {
        public decimal? Price { get; set; }
        public string? Comment { get; set; }
    }
}