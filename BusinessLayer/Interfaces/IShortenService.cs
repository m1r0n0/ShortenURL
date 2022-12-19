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
        Task<LinkViewModelDTO> CreateShortLinkFromFullUrl(LinkViewModelDTO modelDTO, string userName);
        LinkViewModelDTO GetURLsForCurrentUser(LinkViewModelDTO modelDTO, string userName);
        string IdToShortURL(int n);
        string ReverseString(string s);
        int ShortURLToID(string shortUrl);

    }
}
