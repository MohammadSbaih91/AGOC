using AGOC.Models;
using AGOC.ViewModels;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MessageViewModel, Message>()
            .ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.Recipients))
            .ReverseMap();

        CreateMap<MessageRecipientViewModel, MessageRecipient>()
            .ForMember(dest => dest.StatusID, opt => opt.MapFrom(src => src.StatusID)) // Directly map to StatusID
            .ReverseMap();
    }
}
