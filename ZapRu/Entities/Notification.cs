using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapRu.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public int UserID { get; set; }          
        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }

        public int? PartID { get; set; }          
        [ForeignKey(nameof(PartID))]
        public virtual Part Part { get; set; }

        public int? FromUserID { get; set; }      
        [ForeignKey(nameof(FromUserID))]
        public virtual User FromUser { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; } = "bid"; 

        [Required, MaxLength(500)]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool? ResponseStatus { get; set; }
    }
}