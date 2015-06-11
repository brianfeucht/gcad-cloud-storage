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
        private readonly AzureFileStorage fileStorage;

        public AzureFileStorageFixture()
        {
            fileStorage = new AzureFileStorage("unittest");
        }

        [Test]
        public async Task CompletedFileUrl_RandomGuidReturnsNull()
        {
            await fileStorage.EnsureContainerIsCreated();
            var url = await fileStorage.CompletedFileUrl(Guid.NewGuid());
            Assert.IsNull(url);
        }

        [Test]
        public async Task DownloadUserSubmittedFile()
        {
            await fileStorage.EnsureContainerIsCreated();

            var id = Guid.NewGuid();

            var expected = PhotoHelper.GetImageData();
            await fileStorage.UploadUserSubmittedFile(id, expected);

            var actual = await fileStorage.DownloadUserSubmittedFile(id);

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public async Task UploadCompletedFile_ReturnsUrl()
        {
            await fileStorage.EnsureContainerIsCreated();

            var id = Guid.NewGuid();
            
            var url = await fileStorage.UploadCompletedFile(id, PhotoHelper.GetImageData());

            Assert.IsNotNull(url);
        }    
    }
}
