using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeltaBrainJSC.Models
{
    public class DepartmentEmployee
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public Employee Employee { get; set; }
        public Department Department { get; set; }
    }


}
