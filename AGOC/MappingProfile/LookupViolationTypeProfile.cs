using AutoMapper;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.MappingProfile
{
    public class LookupViolationTypeProfile : Profile
    {
        public LookupViolationTypeProfile()
        {
            CreateMap<LookupViolationType, LookupViolationTypeViewModel>().ReverseMap();
        }
    }
}