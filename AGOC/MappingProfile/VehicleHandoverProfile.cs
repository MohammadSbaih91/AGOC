using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class VehicleHandoverProfile : Profile
    {
        public VehicleHandoverProfile()
        {
            CreateMap<VehicleHandover, VehicleHandoverViewModel>().ReverseMap();
        }
    }
}