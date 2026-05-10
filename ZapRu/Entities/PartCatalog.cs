using System.ComponentModel.DataAnnotations;

namespace ZapRu.Entities
{
    public class PartsCatalog
    {
        [Key]
        public int PartCatID { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }

        
        public virtual ICollection<Part> Parts { get; set; }
    }
}
