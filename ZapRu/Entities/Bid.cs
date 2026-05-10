using System.ComponentModel.DataAnnotations;

namespace ZapRu.Entities
{
    public class Bid
    {
        [Key]
        public int BidID { get; set; }

        public int PartID { get; set; }
        public virtual Part Part { get; set; }

        public int BuyerID { get; set; }
        public virtual User Buyer { get; set; }

        public int SellerID { get; set; } 
        public virtual User Seller { get; set; }

        public decimal OfferedPrice { get; set; } 
        public string? Comment { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "pending";

        public int? TransactionID { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
