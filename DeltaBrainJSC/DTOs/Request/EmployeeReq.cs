using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeltaBrainJSC.DTOs.Request
{
    public class EmployeeReq
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string SexReq { get; set; }

        public IFormFile AvatarFile { get; set; }

        [BindNever]
        public byte[] Avatar { get; set; }

        public List<int> DepartmentIds {  get; set; }
    }
}
