using DeltaBrainJSC.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeltaBrainJSC.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress] 
        [MaxLength(255)]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Range(0, 2)]
        public Sex Sex { get; set; }

        public byte[] Avatar { get; set; }
        public virtual ICollection<DepartmentEmployee> DepartmentEmployees { get; set; }
    }

}
