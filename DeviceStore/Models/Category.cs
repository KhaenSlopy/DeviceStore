using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceStore.Models
{
    [Table("Type")]
    public class Category
    {
        [Key]
        public Guid TypeID { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Status { get; set; }
    }
}
