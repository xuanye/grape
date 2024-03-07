using SkiaSharp;
using System;
using System.Collections.Generic;

namespace Grape.Captcha.Steps
{
    internal class LinesDrawStep : IStep
    {
        public void Draw(string text, SKCanvas canvas, CaptchaOptions options)
        {
            if (options.InterferenceLine == null)
            {
                return;
            }

            var descs = GenerateLineGraphicDescriptions(options.Width, options.Height, options.SKForeColors, options.InterferenceLine);

            descs.ForEach(x =>
            {
                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    paint.StrokeWidth = 1;
                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = x.Color.WithAlpha((byte)(255 * x.BlendPercentage));

                    using (SKPath path = new SKPath())
                    {
                        path.MoveTo(x.Start);
                        path.CubicTo(x.Ctrl1, x.Ctrl2, x.End);
                        canvas.DrawPath(path, paint);
                    }
                }
            });
        }

        private List<LineGraphicDescription> GenerateLineGraphicDescriptions(int width, int height, List<SKColor> foreColors, InterferenceLineGeneratorOption interferenceLine)
        {
            var list = new List<LineGraphicDescription>();
            var random = new Random();
            for (var i = 0; i < interferenceLine.Count; i++)
            {
                bool leftInBottom = random.Next(2) == 0;
                int x1 = 5, y1 = leftInBottom ? random.Next(height / 2, height - 5) : random.Next(5, height / 2);
                int x2 = width - 5, y2 = leftInBottom ? random.Next(5, height / 2) : random.Next(height / 2, height - 5);
                int ctrlx1 = random.Next(width / 4, width / 4 * 3), ctrly1 = random.Next(5, height - 5);
                int ctrlx2 = random.Next(width / 4, width / 4 * 3), ctrly2 = random.Next(5, height - 5);
                var colorIndex = random.Next(foreColors.Count);

                list.Add(new LineGraphicDescription
                {
                    Color = foreColors[colorIndex],
                    Start = new SKPoint(x1, y1),
                    Ctrl1 = new SKPoint(ctrlx1, ctrly1),
                    Ctrl2 = new SKPoint(ctrlx2, ctrly2),
                    End = new SKPoint(x2, y2)
                });
            }
            return list;
        }

        private class LineGraphicDescription
        {
            public SKColor Color { get; set; }
            public SKPoint Start { get; set; }
            public SKPoint Ctrl1 { get; set; }
            public SKPoint Ctrl2 { get; set; }
            public SKPoint End { get; set; }
            public float BlendPercentage { get; set; } = 1f;
        }
    }
}