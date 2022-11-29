using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IShortenService
    {
        void GiveUserID(string _name);
        string GetUserID();
        Task<LinkViewModelDTO> CreateLinkPost(LinkViewModelDTO modelDTO);
        Task<LinkViewModelDTO> MyLinksGet(LinkViewModelDTO model_DTO);
        LinkViewModelDTO UseLinkPost(LinkViewModelDTO model_DTO);
    }
}
