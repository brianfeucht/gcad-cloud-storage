using NUnit.Framework;
using Photo.Core;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Core
{
    [TestFixture]
    public class LocalFileStorageFixture
    {
        private readonly string tempFileLocation;
        private readonly LocalFileStorage localFileStorage;
        private readonly byte[] fileBytes;

        public LocalFileStorageFixture()
        {
            tempFileLocation = Path.GetTempPath();
            localFileStorage = new LocalFileStorage(tempFileLocation, "/");
            fileBytes = PhotoHelper.GetImageData();
        }

        [Test]
        public async Task UploadUserSubmittedFile_DownloadUserSubmittedFile_GoldenPath()
        {
            var guid = Guid.NewGuid();
            await localFileStorage.UploadUserSubmittedFile(guid, fileBytes);

            var actual = await localFileStorage.DownloadUserSubmittedFile(guid);

            Assert.IsTrue(fileBytes.SequenceEqual(actual));
        }

        [Test]
        public async Task UploadCompletedFile_CompletedFileUrlIsNotNull()
        {
            var guid = Guid.NewGuid();

            await localFileStorage.UploadCompletedFile(guid, fileBytes);

            var downloadPath = await localFileStorage.CompletedFileUrl(guid);

            Assert.IsNotNull(downloadPath);
        }
    }
}
