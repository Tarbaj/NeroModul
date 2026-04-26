using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Linq;

public string GetTopColor(System.IO.Stream stream)
{
    using (Image<Rgba32> image = image.Load<Rgba32>(stream))
    {
        var colorCounts = new Dictionary<Rgba32, int>();

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var pixel = image[x, y];
                if (colorCounts.ContainsKey(pixel)) colorCounts[pixel]++;
                else colorCounts.Add(pixel, 1);
            }
        }

        var topColor = colorCounts.OrderByDescending(x => x.Value).First().Key;
        return $"#{topColor.R:X2}{topColor.G:X2}{topColor.B:X2}";
    }
}