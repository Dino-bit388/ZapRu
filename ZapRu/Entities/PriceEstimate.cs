using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapRu.Entities
{
    public class PriceEstimate
    {
        [Key]
        public int PriceEstimateID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("CarModel")]
        public int CarModelID { get; set; }

        public int? Mileage { get; set; }

        [MaxLength(50)]
        public string Condition { get; set; }

        [MaxLength(100)]
        public string Region { get; set; }

        public decimal EstimatedValue { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Year { get; set; }           

        
        public virtual User User { get; set; }
        public virtual CarModel CarModel { get; set; }
    }
}
