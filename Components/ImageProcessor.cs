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
        public string GetTopColor(System.IO.Stream stream)
        {
            if (stream.CanSeek) stream.Position = 0;
            using (Image<Rgb24> image = Image.Load<Rgb24>(stream))
            {
                var colorCounts = new Dictionary<Rgb24, int>();
                image.Mutate(x => x.Resize(100, 100));
                image.ProcessPixelRows(a =>
                {
                    for (int y = 0; y < a.Height; y++)
                    {
                        Span<Rgb24> pixelRow = a.GetRowSpan(y);
                        foreach (ref Rgb24 pixel in pixelRow)
                        {
                            if (colorCounts.TryGetValue(pixel, out int count))
                                colorCounts[pixel] = count + 1;
                            else
                                colorCounts[pixel] = 1;
                        }
                    }
                });
                if (colorCounts.Count() == 0) return "#000000";
                var topColor = colorCounts.OrderByDescending(x => x.Value).First().Key;
                return $"#{topColor.R:X2}{topColor.G:X2}{topColor.B:X2}";
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