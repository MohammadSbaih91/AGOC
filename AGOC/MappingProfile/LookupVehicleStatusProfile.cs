using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class LookupVehicleStatusProfile : Profile
    {
        public LookupVehicleStatusProfile()
        {
            CreateMap<LookupVehicleStatus, LookupVehicleStatusViewModel>().ReverseMap();
        }
    }
}