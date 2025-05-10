using AutoMapper;
using DeltaBrainJSC.DTOs.Request;
using DeltaBrainJSC.DTOs.Response;
using DeltaBrainJSC.Models;

namespace DeltaBrainJSC.AutoMapper
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper() 
        {
            CreateMap<EmployeeReq, Employee>();
            CreateMap<Employee, EmployeeRes>();
        }
    }
}
