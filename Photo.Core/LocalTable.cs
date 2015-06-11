using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;

namespace Photo.Core
{
    public class LocalTable : ICloudTable
    {
        public async Task<Guid> Add(CompletedMeme value)
        {
            if (value.Id == default(Guid))
                value.Id = Guid.NewGuid();

            if (value.CreatedOn == default(DateTime))
                value.CreatedOn = DateTime.UtcNow;

            throw new NotImplementedException();

            return value.Id;
        }

        public Task<CompletedMeme> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CompletedMeme>> Latest(int skip = 0, int limit = 10)
        {
            throw new NotImplementedException();
        }
    }
}
