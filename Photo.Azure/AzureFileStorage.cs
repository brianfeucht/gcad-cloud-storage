using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Azure
{
    public class AzureFileStorage : ICloudFileStorage
    {
        public Task<string> CompletedFileUrl(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadUserSubmittedFile(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadCompletedFile(Guid id, byte[] v)
        {
            throw new NotImplementedException();
        }

        public Task UploadUserSubmittedFile(Guid guid, byte[] image)
        {
            throw new NotImplementedException();
        }
    }
}
