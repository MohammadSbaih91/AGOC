using AGOC.Models;
using AGOC.ViewModels;

using AutoMapper;

namespace AGOC.MappingProfile
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageViewModel>().ReverseMap();
        }
    }
}
