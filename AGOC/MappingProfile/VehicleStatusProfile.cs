using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class VehicleStatusProfile : Profile
    {
        public VehicleStatusProfile()
        {
            CreateMap<VehicleStatus, VehicleStatusViewModel>().ReverseMap();
        }
    }
}