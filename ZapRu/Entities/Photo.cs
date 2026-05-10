using System.ComponentModel.DataAnnotations;

namespace ZapRu.Entities
{
    public class Photo
    {
        [Key]
        public int PhotoID { get; set; }

        [Required, MaxLength(10)]
        public string EntityType { get; set; } 

        public int EntityID { get; set; }

        [Required, MaxLength(500)]
        public string FilePath { get; set; }

        public DateTime Uploaded_At { get; set; }
    }
}
