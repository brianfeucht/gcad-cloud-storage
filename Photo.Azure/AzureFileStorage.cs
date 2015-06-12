using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace Photo.Azure
{
    public class AzureFileStorage : ICloudFileStorage
    {
        private bool hasContainerBeenCreated = false;
        private readonly string containerName;

        private CloudBlobContainer Container
        {
            get
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                    ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                var blobClient = storageAccount.CreateCloudBlobClient();
                return blobClient.GetContainerReference(containerName);
            }
        }

        public AzureFileStorage(string containerName)
        {
            this.containerName = containerName;
        }

        public async Task EnsureContainerIsCreated()
        {
            if (hasContainerBeenCreated)
                return;

            await Container.CreateIfNotExistsAsync();

            await Container.SetPermissionsAsync(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            hasContainerBeenCreated = true;
        }

        public async Task<string> CompletedFileUrl(Guid id)
        {
            var completedId = string.Format("{0}_completed.png", id);
            var reference = Container.GetBlockBlobReference(completedId);

            if (!await reference.ExistsAsync())
                return null;

            return reference.Uri.ToString();
        }

        public async Task<byte[]> DownloadUserSubmittedFile(Guid id)
        {
            var reference = Container.GetBlockBlobReference(id.ToString());

            if (!await reference.ExistsAsync())
                return null;

            reference.FetchAttributes();

            var bytes = new byte[reference.Properties.Length];
            await reference.DownloadToByteArrayAsync(bytes, 0);
            return bytes;
        }

        public async Task<string> UploadCompletedFile(Guid id, byte[] imageBytes)
        {
            var completedId = string.Format("{0}_completed.png", id);
            var reference = Container.GetBlockBlobReference(completedId);

            await reference.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

            reference.Properties.ContentType = "image/png";
            await reference.SetPropertiesAsync();

            return reference.Uri.ToString();
        }

        public async Task UploadUserSubmittedFile(Guid id, byte[] imageBytes)
        {
            var reference = Container.GetBlockBlobReference(id.ToString());

            await reference.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
        }
    }
}
