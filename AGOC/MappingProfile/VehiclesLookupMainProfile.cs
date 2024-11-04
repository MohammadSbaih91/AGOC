using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class VehiclesLookupMainProfile : Profile
    {
        public VehiclesLookupMainProfile()
        {
            CreateMap<VehiclesLookupMain, VehiclesLookupMainIndexViewModel>()
            .ForMember(dest => dest.CreateVehicleModel, opt => opt.MapFrom(src => src))
            .ReverseMap();

            CreateMap<VehiclesLookupMain, VehiclesLookupMain>()
            .ReverseMap();
        }
    }
}