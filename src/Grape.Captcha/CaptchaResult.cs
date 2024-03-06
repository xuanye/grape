using System;
using System.Collections.Generic;
using System.Text;

namespace Grape.Captcha
{
    public class CaptchaResult
    {
        public string CaptchaCode { get; set; }

        public string CaptchaValue { get; set; }

        public byte[] Data { get; set; }
    }
}