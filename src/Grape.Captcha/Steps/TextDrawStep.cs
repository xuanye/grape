using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Grape.Captcha.Steps
{
    public class TextDrawStep : IStep
    {
        public void Draw(string text, SKCanvas canvas, CaptchaOptions options)
        {
            var descs = GenerateTextGraphicDescriptions(options.Width, options.Height, text, options.SKFont, options.FontSize, options.SKForeColors, options.TextBold);
            descs.ForEach(x =>
            {
                using (var paint = new SKPaint())
                {
                    paint.StrokeWidth = 1;
                    paint.TextSize = x.FontSize;
                    paint.IsAntialias = true;
                    paint.Typeface = x.Font;
                    paint.Color = x.Color.WithAlpha((byte)(255 * x.BlendPercentage));
                    paint.FakeBoldText = x.TextBold;
                    canvas.DrawText(x.Text, x.Location.X, x.Location.Y, paint);
                }
            });
        }

        private List<TextGraphicDescription> GenerateTextGraphicDescriptions(int width, int height, string text, SKTypeface font, float fontSize, List<SKColor> foreColors, bool isBold)
        {
            var random = new Random();
            var list = new List<TextGraphicDescription>();
            var textPositions = MeasureTextPositions(width, height, text, font, fontSize);
            for (var i = 0; i < text.Count(); i++)
            {
                var colorIndex = random.Next(foreColors.Count);

                list.Add(new TextGraphicDescription
                {
                    Text = text[i].ToString(),
                    Font = font,
                    Color = foreColors[colorIndex],
                    Location = textPositions[i],
                    FontSize = fontSize,
                    TextBold = isBold
                });
            }
            return list;
        }

        /// <summary>
        /// 测算文本绘制位置
        /// </summary>
        /// <param name="width">验证码宽度</param>
        /// <param name="height">验证码高度</param>
        /// <param name="text">要绘制的文本</param>
        /// <param name="paint">画笔</param>
        /// <returns>返回每个字符的位置</returns>
        private List<PointF> MeasureTextPositions(int width, int height, string text, SKTypeface font, float fontSize)
        {
            using (var paint = new SKPaint())
            {
                paint.StrokeWidth = 1;
                paint.TextSize = fontSize;
                paint.IsAntialias = true;
                paint.Typeface = font;

                var result = new List<PointF>();
                if (string.IsNullOrWhiteSpace(text)) return result;

                // 计算每个字符宽度
                var charWidths = new List<float>();
                foreach (var s in text)
                {
                    var charWidth = paint.MeasureText(s.ToString());
                    charWidths.Add(charWidth);
                }

                // 计算每个字符x坐标
                var currentX = 0.0f;
                var charTotalWidth = charWidths.Sum(x => x);
                var charXs = new List<float>();

                // 计算字体高度（取最高的）
                SKRect textBounds = new SKRect();
                paint.MeasureText(text, ref textBounds);
                var fontHeight = (int)textBounds.Height;

                for (var i = 0; i < text.Count(); i++)
                {
                    // 根据文字宽度比例，计算文字包含框宽度
                    var wrapperWidth = charWidths[i] * 1.0f / charTotalWidth * width;
                    // 文字在包含框内的padding
                    var padding = (wrapperWidth - charWidths[i]) / 2;
                    var textX = currentX + padding;
                    int textY = (height + fontHeight) / 2;  // 文字的纵坐标
                    result.Add(new PointF(textX, textY));
                    currentX += wrapperWidth;
                }

                return result;
            }
        }

        private class TextGraphicDescription
        {
            public string Text { get; set; }
            public SKTypeface Font { get; set; }
            public SKColor Color { get; set; }
            public PointF Location { get; set; }
            public float FontSize { get; set; }
            public float BlendPercentage { get; set; } = 1f;
            public bool TextBold { get; set; }
        }
    }
}