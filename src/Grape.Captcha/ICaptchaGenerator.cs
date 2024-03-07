using System.Threading.Tasks;

namespace Grape.Captcha
{
    public interface ICaptchaGenerator
    {
        Task<CaptchaResult> GenerateCaptchaAsync(string captchaCode);
    }
}