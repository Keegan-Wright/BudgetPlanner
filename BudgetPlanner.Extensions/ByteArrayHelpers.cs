using SkiaSharp;
using Svg.Skia;

namespace BudgetPlanner.Extensions
{
    public static class ByteArrayHelpers
    {
        public static MemoryStream ConvertSvgStreamToPngStream(byte[] bytes)
        {
            using var originalStream = new MemoryStream(bytes);
            using var svg = new SKSvg();
            svg.Load(originalStream);


            var pngStream = new MemoryStream();
            svg.Save(pngStream,SKColor.Empty, SKEncodedImageFormat.Png);
            pngStream.Seek(0, SeekOrigin.Begin);

            return pngStream;
        }
    }
}
