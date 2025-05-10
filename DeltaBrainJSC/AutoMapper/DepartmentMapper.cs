using AutoMapper;
using DeltaBrainJSC.DTOs.Request;
using DeltaBrainJSC.DTOs.Response;
using DeltaBrainJSC.Models;

namespace DeltaBrainJSC.AutoMapper
{
    public class DepartmentMapper : Profile
    {
        public DepartmentMapper()
        {
            CreateMap<DepartmentReq, Department>();
            CreateMap<Department, DepartmentRes>();
        }
    }
}
