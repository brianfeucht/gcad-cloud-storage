using Nito.AsyncEx;
using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Photo.Core.Models;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Photo.Core
{
    public class MemeGenerator : IMemeGenerator
    {
        private readonly ICloudFileStorage fileStorage;
        private readonly ICloudTable tableStorage;

        public MemeGenerator(ICloudFileStorage fileStorage, ICloudTable tableStorage)
        {
            this.fileStorage = fileStorage;
            this.tableStorage = tableStorage;
        }

        public async Task GenerateMeme(MemeRequest memeRequest)
        {
            var imageBytes = await fileStorage.DownloadUserSubmittedFile(memeRequest.Id);

            string url;

            using (var memoryStream = new MemoryStream(imageBytes))
            using (var image = Image.FromStream(memoryStream))
            {
                using (var graphics = Graphics.FromImage(image))
                using (Font font = new Font("Comic Sans MS", 24))
                {
                    graphics.DrawString(memeRequest.Text, font, Brushes.Black, new PointF(30f, 30f));
                }

                using (var memorySteam = new MemoryStream())
                {
                    image.Save(memorySteam, ImageFormat.Png);

                    url = await fileStorage.UploadCompletedFile(memeRequest.Id, memorySteam.ToArray());
                }
            }

            var completedMeme = new CompletedMeme()
            {
                CreatedOn = DateTime.UtcNow,
                Id = memeRequest.Id,
                Text = memeRequest.Text,
                ImageUrl = url
            };

            await tableStorage.Add(completedMeme);
        }
    }
}
