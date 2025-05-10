using System.Collections.Generic;


namespace DeltaBrainJSC.DTOs.Response
{
    public class DepartmentRes
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public List<EmployeeRes> Employees { get; set; }
    }
}
