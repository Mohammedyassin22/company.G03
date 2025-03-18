using AutoMapper;
using company.G03.DAL.Models;
using company.G03.PL.Models;

namespace company.G03.PL.Mapping
{
    public class DeptProfile:Profile
    {
        public DeptProfile() {
            CreateMap<CreateDeptDto, Department>();
        }
    }
}
