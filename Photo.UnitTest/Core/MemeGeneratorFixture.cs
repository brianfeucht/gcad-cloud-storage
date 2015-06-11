using Moq;
using NUnit.Framework;
using Photo.Core;
using Photo.Core.Interfaces;
using Photo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Core
{
    [TestFixture]
    public class MemeGeneratorFixture
    {
        private readonly MemeGenerator memeGenerator;
        private readonly Mock<ICloudFileStorage> mockFileStorage;
        private readonly Mock<ICloudTable> mockTableStore;

        public MemeGeneratorFixture()
        {
            mockFileStorage = new Mock<ICloudFileStorage>();
            mockTableStore = new Mock<ICloudTable>();
            memeGenerator = new MemeGenerator(mockFileStorage.Object, mockTableStore.Object);
        } 

        [Test]
        public async Task GenerateMeme_GoldenPath()
        {
            var memeRequest = new MemeRequest()
            {
                Id = Guid.NewGuid()
            };

            mockFileStorage.Setup(m => m.DownloadUserSubmittedFile(memeRequest.Id)).ReturnsAsync(PhotoHelper.GetImageData());

            await memeGenerator.GenerateMeme(memeRequest);

            mockFileStorage.Verify(m => m.UploadCompletedFile(memeRequest.Id, It.Is<byte[]>(b => b.Length > 0)));
            mockTableStore.Verify(m => m.Add(It.Is<CompletedMeme>(meme => meme.Id == memeRequest.Id)));
        }
    }
}
