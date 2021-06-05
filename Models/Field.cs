using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class FieldObj
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Key { get; set; }
        public string Field { get; set; }
        public bool IsRequired { get; set; }
        public long MaxLength { get; set; }
        public string Source { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}