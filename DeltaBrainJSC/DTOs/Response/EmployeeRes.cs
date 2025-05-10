using DeltaBrainJSC.Enum;

namespace DeltaBrainJSC.DTOs.Response
{
    public class EmployeeRes
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public byte[] Avatar { get; set; }

    }
}
