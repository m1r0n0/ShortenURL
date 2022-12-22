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
        Task<LinkViewModelDTO> CreateShortLinkFromFullUrl(LinkViewModelDTO modelDTO, string userName);
        LinkViewModelDTO GetURLsForCurrentUser(LinkViewModelDTO modelDTO, string userName);
        string IdToShortURL(int n);
        int ShortURLToID(string shortUrl);

    }
}
