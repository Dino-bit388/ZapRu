using System.ComponentModel.DataAnnotations;

namespace ZapRu.Entities
{
    public class CarModel
    {
        [Key]
        public int CarModelID { get; set; }

        [Required, MaxLength(100)]
        public string Brand { get; set; }

        [Required, MaxLength(100)]
        public string Model { get; set; }

        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }

       
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<PriceEstimate> PriceEstimates { get; set; }
    }
}
