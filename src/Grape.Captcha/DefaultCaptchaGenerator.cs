using Grape.Captcha.Steps;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grape.Captcha
{
    /// <summary>
    /// 基于SkiaSharp的验证码生成器
    /// </summary>
    public class DefaultCaptchaGenerator : ICaptchaGenerator
    {
        private readonly CaptchaGeneratorOptions _generatorOptions;

        private readonly List<IStep> _steps;

        public DefaultCaptchaGenerator(CaptchaGeneratorOptions generatorOptions)
        {
            _generatorOptions = generatorOptions;
            _steps = new List<IStep>()
            {
                new BgColorDrawStep(),
                new BubblesDrawStep(),
                new LinesDrawStep(),
                new TextDrawStep(),
            };
        }

        public Task<byte[]> GenerateCaptchaAsync(string text)
        {
            byte[] result = null;
            using (var bitmap = new SKBitmap(_generatorOptions.Width, _generatorOptions.Height, false))
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    foreach (var step in _steps)
                    {
                        step.Draw(text, canvas, _generatorOptions);
                    }
                    using (SKData p = bitmap.Encode(SKEncodedImageFormat.Jpeg, _generatorOptions.Quality))
                    {
                        result = p.ToArray();
                    }
                }
            }
            return Task.FromResult(result);
        }
    }
}