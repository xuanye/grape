using Grape.Captcha.AnimatedGif;
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
            byte[] buf;
            if (_generatorOptions.Animation)
            {
                buf = await GenerateCaptchaAnimatedGifAsync(renderText);
            }
            else
            {
                buf = await GenerateCaptchaImageAsync(renderText);
            }

            return new CaptchaResult()
            {
                CaptchaCode = captchaCode,
                CaptchaValue = value,
                Animation = _generatorOptions.Animation,
                Data = buf,
            };
        }

        private Task<byte[]> GenerateCaptchaAnimatedGifAsync(string text)
        {
            AnimatedGifEncoder gifEncoder = new AnimatedGifEncoder();
            gifEncoder.Start();
            gifEncoder.SetDelay(_generatorOptions.FrameDuration);
            //-1:no repeat,0:always repeat
            gifEncoder.SetRepeat(0);
            for (var i = 0; i < text.Length; i++)
            {
                using (var bitmap = new SKBitmap(_generatorOptions.Width, _generatorOptions.Height, false))
                {
                    using (var canvas = new SKCanvas(bitmap))
                    {
                        foreach (var step in _steps)
                        {
                            step.Draw(text, canvas, _generatorOptions, i);
                        }

                        using (SKData skData = bitmap.Encode(SKEncodedImageFormat.Jpeg, _generatorOptions.Quality))
                        {
                            using (var image = SKImage.FromEncodedData(skData))
                            {
                                gifEncoder.AddFrame(image);
                            }
                        }
                    }
                }
            }
            gifEncoder.Finish();
            var stream = gifEncoder.Output();
            return Task.FromResult(stream.ToArray());
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
                        step.Draw(text, canvas, _generatorOptions, 0);
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