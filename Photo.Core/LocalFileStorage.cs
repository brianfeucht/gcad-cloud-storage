using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Core
{
    public class LocalFileStorage : ICloudFileStorage
    {
        private const string CompletedFileFormat = "{0}_completed.png";

        private string imageUrlPath;
        private readonly string rootFilePath;

        public LocalFileStorage(string rootFilePath, string imageUrlPath)
        {
            this.rootFilePath = rootFilePath;
            this.imageUrlPath = imageUrlPath;

            if (!Directory.Exists(rootFilePath))
                Directory.CreateDirectory(rootFilePath);
        }

        public Task<string> CompletedFileUrl(Guid guid)
        {
            var imageLocalPath = GetCompletedFilePath(guid);

            if (!File.Exists(imageLocalPath))
            {
                return Task.FromResult<string>(null);
            }

            return Task.FromResult(string.Format("{0}/{1}", imageUrlPath, string.Format(CompletedFileFormat, guid)));
        }

        public Task<byte[]> DownloadUserSubmittedFile(Guid id)
        {
            var imageLocalPath = GetUserUploadedFilePath(id);

            if (!File.Exists(imageLocalPath))
                return null;

            return Task.FromResult(File.ReadAllBytes(imageLocalPath));
        }

        public async Task<string> UploadCompletedFile(Guid id, byte[] image)
        {
            var imageLocalPath = GetCompletedFilePath(id);
            using (var stream = new FileStream(imageLocalPath, FileMode.Create))
            {
                await stream.WriteAsync(image, 0, image.Length);
            }

            return await CompletedFileUrl(id);
        }

        public async Task UploadUserSubmittedFile(Guid guid, byte[] image)
        {
            var imageLocalPath = GetUserUploadedFilePath(guid);
            using (var stream = new FileStream(imageLocalPath, FileMode.Create))
            {
                await stream.WriteAsync(image, 0, image.Length);
            }
        }

        private string GetCompletedFilePath(Guid guid)
        {
            return Path.Combine(rootFilePath, string.Format(CompletedFileFormat, guid));

        }
        private string GetUserUploadedFilePath(Guid guid)
        {
            return Path.Combine(rootFilePath, string.Format("{0}_pending", guid));
        }
    }
}
