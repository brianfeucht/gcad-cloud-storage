using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;

namespace Photo.Core
{
    public class MemeRepository : IMemeRepository
    {
        public Task<Guid> CreateNewMeme(NewMeme meme)
        {
            // Upload bytes to storage

            // Send message to processor

            throw new NotImplementedException();
        }

        public Task<Uri> GetCompletedMemeUri(Guid guid)
        {
            // See if processor has completed processing image

            // Return url if so otherwise null
            throw new NotImplementedException();
        }
    }
}
