using System.ComponentModel.DataAnnotations;

namespace DeltaBrainJSC.DTOs.Request
{
    public class DepartmentReq
    {
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
