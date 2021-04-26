using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Grape.Captcha
{
    public class CaptchaGeneratorOptions
    {

        /// <summary>
        /// Captcha Font Family
        /// </summary>
        public FontFamily CaptchaFontFamily { get; set; }


        /// <summary>
        /// 只使用数字验证码
        /// </summary>
        public bool OnlyNumber { get; set; }
    }
}
