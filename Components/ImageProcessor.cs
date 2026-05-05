using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.FileSystem;
using System.IO;
using picturpictur.Data;

namespace picturpictur.Components
{
    public class ImageProcessor
    {
        private static readonly Dictionary<string, (Rgb24 Color, string Hex)> Palette = new Dictionary<string, (Rgb24, string)>
        {
            { "Piros", (new Rgb24(227, 34, 25), "#E32219") },
            { "Kék", (new Rgb24(33, 94, 219), "#215EDB") },
            { "Nude", (new Rgb24(232, 198, 159), "#E8C69F") },
            { "Rózsaszín", (new Rgb24(249, 183, 194), "#F9B7C2") },
            { "Pink", (new Rgb24(235, 52, 148), "#EB3494") },
            { "Fehér", (new Rgb24(255, 255, 255), "#FFFFFF") },
            { "Lila", (new Rgb24(125, 31, 199), "#7D1FC7") },
            { "Zöld", (new Rgb24(25, 163, 55), "#19A337") },
            { "Menta", (new Rgb24(152, 222, 194), "#98DEC2") },
            { "Narancs", (new Rgb24(255, 111, 0), "#FF6F00") },
            { "Barack", (new Rgb24(255, 179, 138), "#FFB38A") },
            { "Barna", (new Rgb24(99, 45, 8), "#632D08") },
            { "Szürke", (new Rgb24(110, 117, 130), "#6E7582") },
            { "Arany", (new Rgb24(199, 158, 68), "#C79E44") },
            { "Ezüst", (new Rgb24(186, 186, 186), "#BABABA") }
        };
        private string FindClosestPaletteName(Rgb24 pixel)
        {
            string closestName = "Fehér";
            double minDistanceSquared = double.MaxValue;

            foreach (var entry in Palette)
            {
                var target = entry.Value.Color;

                double diffR = pixel.R - target.R;
                double diffG = pixel.G - target.G;
                double diffB = pixel.B - target.B;
                double distanceSquared = (diffR * diffR) + (diffG * diffG) + (diffB * diffB);

                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    closestName = entry.Key;
                }
            }
            return closestName;
        }
        public string GetTopColor(System.IO.Stream stream)
        {
            if (stream.CanSeek) stream.Position = 0;

            using (Image<Rgb24> image = Image.Load<Rgb24>(stream))
            {
                var colorCounts = Palette.Keys.ToDictionary(name => name, name => 0);
                image.Mutate(x => x.Resize(64, 64));

                image.ProcessPixelRows(accessor =>
                {
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        Span<Rgb24> pixelRow = accessor.GetRowSpan(y);
                        foreach (ref Rgb24 pixel in pixelRow)
                        {
                            string closestName = FindClosestPaletteName(pixel);
                            colorCounts[closestName]++;
                        }
                    }
                });
                var winningName = colorCounts.OrderByDescending(x => x.Value).First().Key;
                return Palette[winningName].Hex;
            }
        }

        public int UploadImage(HttpPostedFileBase file, int portalId)
        {
            var folderManager = FolderManager.Instance;
            var fileManager = FileManager.Instance;

            const string folderPath = "pictur/";
            IFolderInfo folder = folderManager.GetFolder(portalId, folderPath)
                ?? folderManager.AddFolder(portalId, folderPath);
            string safeName = Path.GetFileName(file.FileName)
                .Replace(" ", "_");
            IFileInfo savedFile = fileManager.AddFile(
                folder,
                safeName,
                file.InputStream,
                overwrite: true);
            return savedFile.FileId;
        }

        public void DeleteImage(int fileId)
        {
            var fileInfo = FileManager.Instance.GetFile(fileId);
            if (fileInfo != null)
            {
                FileManager.Instance.DeleteFile(fileInfo);
                if (System.IO.File.Exists(fileInfo.PhysicalPath))
                {
                    System.IO.File.Delete(fileInfo.PhysicalPath);
                }
            }
        }
    }
}