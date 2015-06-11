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
        private readonly ICloudFileStorage fileStorage;
        private readonly ICloudQueue queue;
        private readonly ICloudTable table;

        public MemeRepository(ICloudFileStorage fileStorage, ICloudQueue queue, ICloudTable table)
        {
            this.fileStorage = fileStorage;
            this.queue = queue;
            this.table = table;
        }

        public async Task<Guid> CreateNewMeme(NewMeme meme)
        {
            var guid = Guid.NewGuid();

            // Upload bytes to storage
            await fileStorage.UploadUserSubmittedFile(guid, meme.Image);

            var memeRequest = new MemeRequest()
            {
                Id = guid,
                Text = meme.Text
            };

            // Send message to processor
            await queue.EnqueueMessage(memeRequest);

            return guid;
        }

        public async Task<string> GetCompletedMemeUri(Guid guid)
        {
            return await fileStorage.CompletedFileUrl(guid);
        }

        public async Task<IEnumerable<CompletedMeme>> LatestCompletedMemes()
        {
            return await table.Latest();
        }
    }
}
