using Svg;
using System.Drawing.Imaging;
using System.Security.Principal;

namespace BudgetPlanner.Extensions
{
    public static class ByteArrayHelpers
    {
        public static MemoryStream ConvertSvgStreamToPngStream(byte[] bytes)
        {
            using var originalStream = new MemoryStream(bytes);
            var svgDoc = SvgDocument.Open<SvgDocument>(originalStream);

            using var svgAsBmp = svgDoc.Draw();

            var pngStream = new MemoryStream();
            svgAsBmp.Save(pngStream, ImageFormat.Png);
            pngStream.Seek(0, SeekOrigin.Begin);

            return pngStream;
        }
    }
}
