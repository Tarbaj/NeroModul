using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}