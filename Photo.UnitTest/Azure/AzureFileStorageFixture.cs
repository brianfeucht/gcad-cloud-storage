using NUnit.Framework;
using Photo.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Azure
{
    [TestFixture]
    public class AzureFileStorageFixture
    {
        [Test]
        public async Task CompletedFileUrl_RandomGuidReturnsNull()
        {
            var fileStorage = new AzureFileStorage();
            var url = await fileStorage.CompletedFileUrl(Guid.NewGuid());
            Assert.IsNull(url);
        }

        [Test]
        public async Task DownloadUserSubmittedFile()
        {
            var id = Guid.NewGuid();

            var fileStorage = new AzureFileStorage();

            var expected = PhotoHelper.GetImageData();
            await fileStorage.UploadUserSubmittedFile(id, expected);

            var actual = await fileStorage.DownloadUserSubmittedFile(id);

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public async Task UploadCompletedFile_ReturnsUrl()
        {
            var id = Guid.NewGuid();

            var fileStorage = new AzureFileStorage();
            var url = await fileStorage.UploadCompletedFile(id, PhotoHelper.GetImageData());

            Assert.IsNotNull(url);
        }    
    }
}
