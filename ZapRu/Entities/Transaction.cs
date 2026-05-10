using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapRu.Entities
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

       
        public int PartID { get; set; }

        
        public int BuyerID { get; set; }

        
        public int SellerID { get; set; }

        public decimal AgreedPrice { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        
        public virtual Part Part { get; set; }
        public virtual User Buyer { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;
        public virtual Contract Contract { get; set; } 
    }
}
