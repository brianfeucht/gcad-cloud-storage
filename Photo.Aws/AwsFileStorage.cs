using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Aws
{
    public class AwsFileStorage : ICloudFileStorage
    {
        private const string BucketName = "gcad-storage-demo";
        private AmazonS3Client s3Client;

        public AwsFileStorage()
        {
            var credentialFactory = new AwsCredentialFactory();

            s3Client = new AmazonS3Client(credentialFactory.Credentials(), RegionEndpoint.USWest2);
        }

        public async Task<string> CompletedFileUrl(Guid guid)
        {
            var completed = string.Format("{0}_complated.png", guid);

            try
            {
                var metaData = await s3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest()
                {
                    BucketName = BucketName,
                    Key = completed
                });
            }
            catch(AmazonS3Exception ex)
            {
                if(ex.StatusCode == HttpStatusCode.Forbidden || ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }

            return "https://s3-us-west-2.amazonaws.com/" + BucketName + "/" + completed;
        }

        public async Task<byte[]> DownloadUserSubmittedFile(Guid id)
        {
            var response = await s3Client.GetObjectAsync(new GetObjectRequest()
            {
                BucketName = BucketName,
                Key = id.ToString()
            });

            using (var memoryStream = new MemoryStream())
            {
                await response.ResponseStream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<string> UploadCompletedFile(Guid id, byte[] image)
        {
            var completed = string.Format("{0}_complated.png", id);

            using (var memoryStream = new MemoryStream(image))
            {
                var response = await s3Client.PutObjectAsync(new PutObjectRequest()
                {
                    BucketName = BucketName,
                    Key = completed,
                    InputStream = memoryStream
                });
            }
            
            return "https://s3-us-west-2.amazonaws.com/" + BucketName + "/" + completed;
        }

        public async Task UploadUserSubmittedFile(Guid guid, byte[] image)
        {
            using (var memoryStream = new MemoryStream(image))
            {
                var response = await s3Client.PutObjectAsync(new PutObjectRequest()
                {
                    BucketName = BucketName,
                    Key = guid.ToString(),
                    InputStream = memoryStream
                });
            }
        }

    }
}
