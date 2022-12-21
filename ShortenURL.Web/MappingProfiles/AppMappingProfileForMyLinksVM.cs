using AutoMapper;
using BusinessLayer.DTOs;
using ShortenURL.Models;

namespace ShortenURL.MappingProfiles
{
    public class AppMappingProfileForMyLinksVM : Profile
    {
        public AppMappingProfileForMyLinksVM()
        {
            CreateMap<LinkViewModelDTO, MyLinksViewModel>().ReverseMap();
        }
    }
}
