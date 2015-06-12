using NUnit.Framework;
using Photo.Aws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Aws
{
    [TestFixture]
    public class AwsFileStorageFixture
    {
        private readonly AwsFileStorage fileStorage;

        public AwsFileStorageFixture()
        {
            fileStorage = new AwsFileStorage();
        }

        [Test]
        public async Task CompletedFileUrl_RandomGuidReturnsNull()
        {
            var url = await fileStorage.CompletedFileUrl(Guid.NewGuid());
            Assert.IsNull(url);
        }

        [Test]
        public async Task DownloadUserSubmittedFile()
        {
            var id = Guid.NewGuid();

            var expected = PhotoHelper.GetImageData();
            await fileStorage.UploadUserSubmittedFile(id, expected);

            var actual = await fileStorage.DownloadUserSubmittedFile(id);

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public async Task UploadCompletedFile_ReturnsUrl()
        {
            var id = Guid.NewGuid();

            var url = await fileStorage.UploadCompletedFile(id, PhotoHelper.GetImageData());

            Assert.IsNotNull(url);
        }
    }
}
