using DeltaBrainJSC.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeltaBrainJSC.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public virtual ICollection<DepartmentEmployee> DepartmentEmployees { get; set; }
    }

}
