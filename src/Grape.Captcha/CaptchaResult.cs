namespace Grape.Captcha
{
    public class CaptchaResult
    {
        public string CaptchaCode { get; set; }

        public string CaptchaValue { get; set; }

        public bool Animation { get; set; }

        public byte[] Data { get; set; }
    }
}