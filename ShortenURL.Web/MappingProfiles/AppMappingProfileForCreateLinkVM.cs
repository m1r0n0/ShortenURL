using AutoMapper;
using BusinessLayer.DTOs;
using ShortenURL.Models;

namespace ShortenURL.MappingProfiles
{
    public class AppMappingProfileForCreateLinkVM : Profile
    {
        public AppMappingProfileForCreateLinkVM()
        {
            CreateMap<LinkViewModelDTO, CreateLinkViewModel>().ReverseMap();
        }
    }
}
