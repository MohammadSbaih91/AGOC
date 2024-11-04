using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class ParkingProfile : Profile
    {
        public ParkingProfile()
        {
            CreateMap<Parking, ParkingViewModels>().ReverseMap();
        }
    }
}