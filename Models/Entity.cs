using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Key { get; set; }
        public string EntityName { get; set; }
        public IEnumerable<FieldObj> Fields { get; set; }
         public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}