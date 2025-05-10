using System.ComponentModel.DataAnnotations;

namespace DeltaBrainJSC.DTOs.Request
{
    public class DepartmentUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
