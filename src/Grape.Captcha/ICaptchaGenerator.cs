using System.Threading.Tasks;

namespace Grape.Captcha
{
    public interface ICaptchaGenerator
    {
        Task<byte[]> GenerateCaptchaAsync(string text);
    }
}