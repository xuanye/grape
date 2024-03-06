using SkiaSharp;

namespace Grape.Captcha
{
    public interface IStep
    {
        void Draw(string text, SKCanvas canvas, CaptchaGeneratorOptions options);
    }
}