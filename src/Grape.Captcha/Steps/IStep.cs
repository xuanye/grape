using SkiaSharp;

namespace Grape.Captcha.Steps
{
    public interface IStep
    {
        void Draw(string text, SKCanvas canvas, CaptchaOptions options);
    }
}