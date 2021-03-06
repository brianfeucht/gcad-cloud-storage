﻿using Moq;
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
    public class MemeRepositoryFixture
    {
        private readonly Mock<ICloudFileStorage> mockFileStorage;
        private readonly Mock<ICloudQueue> mockQueue;
        private readonly Mock<ICloudTable> mockTable;
        private readonly MemeRepository repository;

        public MemeRepositoryFixture()
        {
            mockFileStorage = new Mock<ICloudFileStorage>();
            mockQueue = new Mock<ICloudQueue>();
            mockTable = new Mock<ICloudTable>();
            repository = new MemeRepository(mockFileStorage.Object, mockQueue.Object, mockTable.Object);
        }

        [Test]
        public async Task CreateNewMeme_GoldenPath()
        {
            var newMeme = new NewMeme()
            {
                Text = "UnitTest_CreateNewMeme_GoldenPath",
                Image = new byte[10]
            };

            await repository.CreateNewMeme(newMeme);

            mockFileStorage.Verify(m => m.UploadUserSubmittedFile(It.IsAny<Guid>(), newMeme.Image));
            mockQueue.Verify(m => m.EnqueueMessage(It.Is<MemeRequest>(r => r.Text == newMeme.Text)));
        }

        [Test]
        public async Task GetCompletedMemeUri_GoldenPath()
        {
            var guid = Guid.NewGuid();
            await repository.GetCompletedMemeUri(guid);

            mockFileStorage.Verify(m => m.CompletedFileUrl(guid));
        }

        [Test]
        public async Task GetLatest_GoldenPath()
        {
            await repository.LatestCompletedMemes();
            mockTable.Verify(m => m.Latest(It.IsAny<int>(), It.IsAny<int>()));
        }
    }
}
