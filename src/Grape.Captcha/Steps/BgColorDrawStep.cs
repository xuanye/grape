using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grape.Captcha.Steps
{
    internal class BgColorDrawStep : IStep
    {
        public void Draw(string text, SKCanvas canvas, CaptchaOptions options, int frameIndex)
        {
            canvas.DrawColor(options.SKBgColor);
        }
    }
}