using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Core.Interfaces
{
    public interface ICloudFileStorage
    {
        Task UploadUserSubmittedFile(Guid id, byte[] image);
        Task<string> CompletedFileUrl(Guid id);
        Task<byte[]> DownloadUserSubmittedFile(Guid id);
        Task<string> UploadCompletedFile(Guid id, byte[] v);
    }
}
