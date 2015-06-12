using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;

namespace Photo.Aws
{
    public class AwsTable : ICloudTable
    {
        public Task<Guid> Add(CompletedMeme value)
        {
            throw new NotImplementedException();
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
