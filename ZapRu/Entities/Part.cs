using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapRu.Entities
{
    public class Part
    {
        [Key]
        public int PartID { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }

        [ForeignKey("PartsCatalog")]
        public int PartCatID { get; set; }

        public decimal? Price { get; set; }

        [MaxLength(50)]
        public string Condition { get; set; } 

        public bool IsSold { get; set; }

        public DateTime? SoldAt { get; set; }

        public int ViewsCount { get; set; }

        
        public virtual Car Car { get; set; }
        public virtual PartsCatalog PartsCatalog { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
