using Grape.Captcha.Steps;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grape.Captcha
{
    /// <summary>
    /// 基于SkiaSharp的验证码图片生成器
    /// </summary>
    public class DefaultCaptchaGenerator : ICaptchaGenerator
    {
        private readonly CaptchaOptions _generatorOptions;

        private readonly List<IStep> _steps;

        public DefaultCaptchaGenerator(CaptchaOptions generatorOptions)
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

        public async Task<CaptchaResult> GenerateCaptchaAsync(string captchaCode)
        {
            var codeGenerator = CaptchaCodeGeneratorFactory.GetCaptchaCodeGenerator(_generatorOptions.CaptchaType);
            var (renderText, value) = codeGenerator.GenerateCode(_generatorOptions.Length);

            var data = await GenerateCaptchaImageAsync(renderText);

            return new CaptchaResult()
            {
                CaptchaCode = captchaCode,
                CaptchaValue = value,
                Data = data
            };
        }

        private Task<byte[]> GenerateCaptchaImageAsync(string text)
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