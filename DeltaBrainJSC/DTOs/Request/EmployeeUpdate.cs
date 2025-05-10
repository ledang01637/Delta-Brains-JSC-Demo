using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeltaBrainJSC.DTOs.Request
{
    public class EmployeeUpdate
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

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
        public List<int> DepartmentIds{ get; set; }

        [BindNever]
        public byte[] Avatar { get; set; }


    }
}
