using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Core.Interfaces
{
    public interface ICloudFileStorage
    {
        Task UploadUserSubmittedFile(Guid guid, byte[] image);
        Task<Uri> CompletedFileUrl(Guid guid);
    }
}
