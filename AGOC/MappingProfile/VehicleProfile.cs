using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleViewModel>().ReverseMap();
        }
    }
}