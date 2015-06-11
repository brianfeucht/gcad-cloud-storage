using Photo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Core.Interfaces
{
    public interface IMemeRepository
    {
        Task<Guid> CreateNewMeme(NewMeme meme);
        Task<string> GetCompletedMemeUri(Guid guid);
        Task<IEnumerable<CompletedMeme>> LatestCompletedMemes();
    }
}
