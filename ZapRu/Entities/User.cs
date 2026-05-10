
using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

namespace ZapRu.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? MiddleName { get; set; } 

        [Required]
        public string PasswordHash { get; set; } 

        public DateTime RegisteredAt { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool IsBlocked { get; set; } = false;

        public void SetPassword(string password) 
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);      
        }

      
        public virtual ICollection<Car> Cars { get; set; }        
        public virtual ICollection<Transaction> BuyerTransactions { get; set; }
        public virtual ICollection<Transaction> SellerTransactions { get; set; }
        public virtual ICollection<PriceEstimate> PriceEstimates { get; set; }
    }
}
