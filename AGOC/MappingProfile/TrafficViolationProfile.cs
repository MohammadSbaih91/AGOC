using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class TrafficViolationProfile : Profile
    {
        public TrafficViolationProfile()
        {
            CreateMap<TrafficViolation, TrafficViolationViewModel>().ReverseMap();
        }
    }
}