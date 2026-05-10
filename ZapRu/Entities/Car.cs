using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapRu.Entities
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [ForeignKey("Seller")]
        public int SellerID { get; set; }

        [ForeignKey("CarModel")]
        public int CarModelID { get; set; }

        [MaxLength(17)]
        public string? VIN { get; set; }

        public int? Mileage { get; set; }

        [MaxLength(50)]
        public string Condition { get; set; } 

        [MaxLength(100)]
        public string Region { get; set; }

        public string Description { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int ManufactureYear { get; set; }

    
        public virtual User Seller { get; set; }
        public virtual CarModel CarModel { get; set; }
        public virtual ICollection<Part> Parts { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
