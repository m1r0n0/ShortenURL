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
        string GetUserIDFromUserName(string name);
        //string GetUserID();
        Task<LinkViewModelDTO> CreateShortLinkFromFullUrl(LinkViewModelDTO modelDTO, string userName);
        LinkViewModelDTO GetURLsForCurrentUser(LinkViewModelDTO modelDTO, string userName);
        //LinkViewModelDTO FindAppropriateLinkInDB(LinkViewModelDTO modelDTO, string userName);
        public string GetLinkToRedirect(LinkViewModelDTO modelDTO, string userName);
    }
}
