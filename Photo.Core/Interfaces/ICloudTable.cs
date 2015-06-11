using Photo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Core.Interfaces
{
    public interface ICloudTable
    {
        Task<Guid> Add(CompletedMeme value);
        Task<CompletedMeme> Get(Guid id);
        Task<IEnumerable<CompletedMeme>> Latest(int skip = 0, int limit = 10);
    }
}
