using SkiaSharp;
using System.Collections.Generic;

namespace Grape.Captcha
{
    public class CaptchaGeneratorOptions
    {
        /// <summary>
        /// 验证码的宽
        /// </summary>
        public int Width { get; set; } = 130;

        /// <summary>
        /// 验证码高
        /// </summary>
        public int Height { get; set; } = 48;

        /// <summary>
        /// 图片质量（仅对静态验证有效）
        /// </summary>
        public int Quality { get; set; } = 100;

        /// <summary>
        /// 文本粗体
        /// </summary>
        public bool TextBold { get; set; } = false;

        public SKTypeface Font { get; set; }
        public int FontSize { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public SKColor BgColor { get; set; } = SKColors.White;

        /// <summary>
        /// 前景色,多个
        /// </summary>
        public List<SKColor> ForeColors { get; set; }

        /// <summary>
        /// 干扰气泡的配置
        /// </summary>
        public InterferenceBubbleGeneratorOption InterferenceBubble { get; set; }

        /// <summary>
        /// 干扰线
        /// </summary>
        public InterferenceLineGeneratorOption InterferenceLine { get; set; }
    }

    public class InterferenceLineGeneratorOption
    {
        /// <summary>
        /// 生成的干扰线数量
        /// </summary>
        public int Count { get; set; } = 1;
    }

    public class InterferenceBubbleGeneratorOption
    {
        /// <summary>
        /// 生成气泡数量
        /// </summary>
        public int Count { get; set; } = 3;

        /// <summary>
        /// 气泡线宽度
        /// </summary>
        public float Thickness { get; set; }

        /// <summary>
        /// 气泡最小半径
        /// </summary>
        public float MinRadius { get; set; } = 3;

        /// <summary>
        /// 气泡最小半径
        /// </summary>
        public float MaxRadius { get; set; } = 8;
    }
}