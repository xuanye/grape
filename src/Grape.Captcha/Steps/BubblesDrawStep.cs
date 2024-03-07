using SkiaSharp;
using System;
using System.Collections.Generic;

namespace Grape.Captcha.Steps
{
    internal class BubblesDrawStep : IStep
    {
        public void Draw(string text, SKCanvas canvas, CaptchaOptions options)
        {
            if (options.InterferenceBubble == null)
            {
                return;
            }
            var descs = GenerateBubbleGraphicDescriptions(options.Width, options.Height, options.SKForeColors, options.InterferenceBubble);

            descs.ForEach(x =>
            {
                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    paint.StrokeWidth = x.Thickness;
                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = x.Color.WithAlpha((byte)(255 * x.BlendPercentage));
                    canvas.DrawCircle(x.Cx, x.Cy, x.Radius, paint);
                }
            });
        }

        private List<BubbleGraphicDescription> GenerateBubbleGraphicDescriptions(int width, int height, List<SKColor> foreColors, InterferenceBubbleGeneratorOption bubbleOption)
        {
            var list = new List<BubbleGraphicDescription>();
            var random = new Random();
            var radiusDiff = bubbleOption.MaxRadius - bubbleOption.MinRadius;
            if (radiusDiff < 1)
            {
                radiusDiff = 1;
            }
            var colorCount = foreColors.Count;
            for (int i = 0; i < bubbleOption.Count; i++)
            {
                var radius = bubbleOption.MinRadius + random.Next(0, 101) / 100 * radiusDiff;
                var cx = random.Next(width - 25) + radius;
                var cy = random.Next(height - 15) + radius;
                var colorIndex = random.Next(colorCount);

                list.Add(new BubbleGraphicDescription()
                {
                    Cx = cx,
                    Cy = cy,
                    Radius = radius,
                    Thickness = bubbleOption.Thickness,
                    Color = foreColors[colorIndex]
                });
            }
            return list;
        }

        private class BubbleGraphicDescription
        {
            public float Cx { get; set; }
            public float Cy { get; set; }
            public float Radius { get; set; }
            public SKColor Color { get; set; }
            public float Thickness { get; set; }
            public float BlendPercentage { get; set; } = 1f;
        }
    }
}