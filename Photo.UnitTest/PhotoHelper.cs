using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest
{
    public class PhotoHelper
    {
        public static byte[] GetImageData()
        {
            //Create the empty image.
            Bitmap image = new Bitmap(50, 50);

            //draw a useless line for some data
            Graphics imageData = Graphics.FromImage(image);
            imageData.DrawLine(new Pen(Color.Red), 0, 0, 50, 50);

            //Convert to byte array
            MemoryStream memoryStream = new MemoryStream();
            byte[] bitmapData;

            using (memoryStream)
            {
                image.Save(memoryStream, ImageFormat.Png);
                bitmapData = memoryStream.ToArray();
            }
            return bitmapData;
        }
    }
}
