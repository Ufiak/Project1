using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MorzkulcPhotoConverterUpgrade
{
   public class PhotoConversionContainer
    {

        /// This method will kick off all necessary operations for converting and storing the images.
        /// </summary>
        /// <param name="myContainer">The container with most of the information needed to perform this operation.</param>
        /// <param name="files">An array of files, which need to be converted. Only needed when dealing with multiple files.</param>
        //public static void ImageConversion(ImageMetaDataContainer myContainer, string[] files) // Private?
            public static void ImageConversion(ImageMetaDataContainer myContainer, string[] files) 
        {
            if (myContainer.input.Equals("single"))
            {
                var photo = Image.FromFile(myContainer.sourceOfFolderAndImage, true);
                var convertedPhoto = ResizeImage(photo, myContainer.width, myContainer.height);
                Format(convertedPhoto, myContainer.destinationFullPath, myContainer.format);
            }
            else
            {
                List<string> destinationFullPathList = new List<string>();

                Console.WriteLine("images in the given directory: \n");
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }

                for (int i = 0; i <= files.Count() - 1; i++)
                {
                    destinationFullPathList.Add(myContainer.destinationFolder + "\\" + myContainer.destinationFile + i + "." + myContainer.format);
                }
                Console.WriteLine("below are images from destinationFullPathList");

                foreach (string path in destinationFullPathList)
                {
                    Console.WriteLine(path);
                }

                for (int i = 0; i <= files.Count() - 1; i++)
                {
                    var newNumber = files.Count();
                    Console.WriteLine("There are: " + newNumber + " ,in given folders.");
                    {
                        var photo = Image.FromFile(files[i], true);
                        var convertedPhoto = ResizeImage(photo, myContainer.width, myContainer.height);
                        Format(convertedPhoto, destinationFullPathList[i], myContainer.format);
                    }
                }
            }
        }
        private static Bitmap ResizeImage(Image image, int width, int height) 
        {
            Bitmap destImage;
            int newWidth = 0;
            int newHeight = 0;
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            if ((height == 0) && (width != 0))
            {
                newWidth = width;
                newHeight = originalHeight * newWidth / originalWidth;
            }
            else if ((height != 0) && (width == 0))
            {
                newHeight = height;
                newWidth = originalWidth * newHeight / originalHeight;
            }
            else if ((height != 0) && (width != 0))
            {
                newWidth = width;
                newHeight = height;
            }

            Console.WriteLine("\noriginalWidth: " + originalWidth);
            Console.WriteLine("originalHeight: " + originalHeight);

            if (Convert.ToDouble(originalWidth) / Convert.ToDouble(originalHeight) == 16 / 9.0)
                Console.WriteLine("original aspect ratio is: 16:9");
            else if (Convert.ToDouble(originalWidth) / Convert.ToDouble(originalHeight) == 4 / 3.0)
                Console.WriteLine("original aspect ratio is: 4:3");
            else if (Convert.ToDouble(originalWidth) / Convert.ToDouble(originalHeight) == 1.41 / 3.0)
                Console.WriteLine("original aspect ratio is of format A4");
            else if (Convert.ToDouble(originalWidth) / Convert.ToDouble(originalHeight) == 1 / 1.0)
                Console.WriteLine("original aspect ratio is: 1:1");
            else
                Console.WriteLine("unable to determine aspect ratio");

            Console.WriteLine("new Width: " + newWidth);
            Console.WriteLine("new Height: " + newHeight);

   
            var destRect = new Rectangle(0, 0, newWidth, newHeight); // Stores a set of four integers that represent the location and size of a rectangle
            destImage = new Bitmap(newWidth, newHeight); // resized Image with newWidth and newHeight

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution); // Sets the resolution for this Bitmap. Mantains resolution of original image.

            using (var graphics = Graphics.FromImage(destImage)) //creates a new graphic from the given image ()
            {
                graphics.CompositingMode = CompositingMode.SourceCopy; //Gets a value that specifies how composited images are drawn to this Graphics. determines whether pixels from a source image overwrite or are combined with background pixels. SourceCopy: specifies that when a color is rendered, it overwrites the background color.
                graphics.CompositingQuality = CompositingQuality.HighQuality; // determines the rendering quality level of layered images.
                graphics.CompositingQuality = CompositingQuality.AssumeLinear;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; // determines how intermediate values between two endpoints are calculated. no reason to do different quality IMO
                graphics.SmoothingMode = SmoothingMode.HighQuality; //antialiasing. Specifies whether lines, curves, and the edges of filled areas use smoothing 
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality; // Gets or set a value specifying how pixels are offset during rendering of this Graphics.

                using (var wrapMode = new ImageAttributes()) // good stuff for border stuff handling, look below. and also draws image out of a photo.
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY); // prevents ghosting around the image borders// wraps image so there are no transparent pixels beyond image borders.
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode); // Draws a resized image, normally in using var graphics /graphics.DrawImage(image, 0, 0, newWidth, newHeight);/-> image-photo; Rectangle class with newWidth and newHeight ; src X, src Y: 0, 0; original width, original height, specifies measure of the given data, wrap mode. 
                }

            }
            return destImage;
        }

        private static void Format(Bitmap convertedPhotoM, string whereToPutThePhotoM, string formatM) 
        {
            switch (formatM)
            {
                case "jpg":
                    var formatNeeded = ImageFormat.Jpeg;
                    convertedPhotoM.Save(whereToPutThePhotoM, formatNeeded);
                    Console.WriteLine("check");
                    if (File.Exists(whereToPutThePhotoM))
                    {
                        Console.WriteLine("Image converted to 'jpg' and is saved to: " + whereToPutThePhotoM);

                    }
                    break;

                case "png":
                    formatNeeded = ImageFormat.Png;
                    convertedPhotoM.Save(whereToPutThePhotoM, formatNeeded);
                    if (File.Exists(whereToPutThePhotoM))
                        Console.WriteLine("Image converted to 'png'and is saved to: " + whereToPutThePhotoM);
                    break;
                case "gif":
                    formatNeeded = ImageFormat.Gif;
                    convertedPhotoM.Save(whereToPutThePhotoM, formatNeeded);
                    if (File.Exists(whereToPutThePhotoM))
                        Console.WriteLine("Image converted! But it's gif and it will suck hard! It is saved to: " + whereToPutThePhotoM);
                    break;
                case "ico":
                    formatNeeded = ImageFormat.Icon;
                    convertedPhotoM.Save(whereToPutThePhotoM, formatNeeded);
                    if (File.Exists(whereToPutThePhotoM))
                        Console.WriteLine("Image converted to 'ico'! and is saved to: " + whereToPutThePhotoM);
                    break;

                default:

                    formatNeeded = ImageFormat.Jpeg;
                    convertedPhotoM.Save(whereToPutThePhotoM, formatNeeded);
                    if (File.Exists(whereToPutThePhotoM))
                        Console.WriteLine("Image converted to 'jpg'! and is saved to: " + whereToPutThePhotoM);
                    break;
            }
        }

    }
}

