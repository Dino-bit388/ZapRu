using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapRu.Entities
{
    public class Contract
    {
        [Key]
        public int ContractID { get; set; }

        [ForeignKey("Transaction")]
        public int TransactionID { get; set; }

        [Required, MaxLength(50)]
        public string ContractNumber { get; set; }

        [MaxLength(500)]
        public string FilePath { get; set; }

        public bool SignedBySeller { get; set; }
        public bool SignedByBuyer { get; set; }

        public DateTime CreatedAt { get; set; }

      
        public virtual Transaction Transaction { get; set; }
    }
}
