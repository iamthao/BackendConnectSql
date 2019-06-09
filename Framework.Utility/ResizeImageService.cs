using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Framework.Utility
{
    public class ResizeImageService : IResizeImage
    {
        public static ImageFormat GetImageFormat(Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            else
                return ImageFormat.Wmf;
        }
        public static byte[] RoundCorners(byte[] startImageByte, int cornerRadius)
        {
            var ms = new MemoryStream(startImageByte);
            var startImage = Image.FromStream(ms);

            cornerRadius *= 2;
            var roundedImage = new Bitmap(startImage.Width, startImage.Height);
            var g = Graphics.FromImage(roundedImage);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var brush = new TextureBrush(startImage);
            var gp = new GraphicsPath();
            gp.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gp.AddArc(0, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            g.FillPath(brush, gp);

            var ms1 = new MemoryStream();

            roundedImage.Save(ms1, GetImageFormat(startImage));
            return ms1.ToArray();
        }


        public byte[] ResizeImageByHeightAndWidth(string filePath, int height, int width)
        {
            var img = new Bitmap(filePath);
               
            var resizedImg = new Bitmap((int)width, height);
            var outStream = new MemoryStream();
            var graphic = Graphics.FromImage(resizedImg);
            graphic.Clear(Color.White);
            graphic.DrawImage(img, new Rectangle(0, 0, (int)width, height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            resizedImg.Save(outStream, GetImageFormat(filePath));
            graphic.Dispose();
            resizedImg.Dispose();
            img.Dispose();
            var byteResult = outStream.ToArray();
            outStream.Dispose();
            return byteResult;
        }
        public byte[] ResizeImageByHeight(string filePath, int height)
        {
            var img = new Bitmap(filePath);
            var scaleRate = (height * 1.0) / (img.Height * 1.0);
            var width = img.Width * scaleRate;
            var resizedImg = new Bitmap((int)width, height);
            var outStream = new MemoryStream();
            var graphic = Graphics.FromImage(resizedImg);
            graphic.Clear(Color.White);
            graphic.DrawImage(img, new Rectangle(0, 0, (int)width, height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            resizedImg.Save(outStream, GetImageFormat(filePath));
            graphic.Dispose();
            resizedImg.Dispose();
            img.Dispose();
            var byteResult = outStream.ToArray();
            outStream.Dispose();
            return byteResult;
        }

        public byte[] ResizeImageByWidth(string filePath, int width)
        {
            var img = new Bitmap(filePath);
            var scaleRate = (width * 1.0) / (img.Width * 1.0);
            var height = img.Height * scaleRate;
            var resizedImg = new Bitmap(width, (int)height);
            var outStream = new MemoryStream();
            var graphic = Graphics.FromImage(resizedImg);
            graphic.Clear(Color.White);
            graphic.DrawImage(img, new Rectangle(0, 0, width, (int)height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            resizedImg.Save(outStream, GetImageFormat(filePath));
            graphic.Dispose();
            resizedImg.Dispose();
            img.Dispose();
            var byteResult = outStream.ToArray();
            outStream.Dispose();
            return byteResult;
        }

        private ImageFormat GetImageFormat(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
            }
            return ImageFormat.Jpeg;
        }
    }
}