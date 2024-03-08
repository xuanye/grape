using SkiaSharp;
using System.Collections.Generic;

namespace Grape.Captcha
{
    public class CaptchaOptions
    {
        public CaptchaCodeGeneratorType CaptchaType { get; set; } = CaptchaCodeGeneratorType.Default;

        public int Length { get; set; } = 4;

        /// <summary>
        /// 验证码的宽
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// 验证码高
        /// </summary>
        public int Height { get; set; } = 42;

        /// <summary>
        /// 图片质量（仅对静态验证有效）
        /// </summary>
        public int Quality { get; set; } = 100;

        /// <summary>
        /// 文本粗体
        /// </summary>
        public bool TextBold { get; set; } = false;

        public string FontFamily { get; set; }

        private SKTypeface _font;

        public SKTypeface SKFont
        {
            get
            {
                if (_font == null)
                {
                    if (string.IsNullOrWhiteSpace(this.FontFamily))
                    {
                        _font = DefaultFontFamilys.Instance.Actionj;
                    }
                    else
                    {
                        _font = DefaultFontFamilys.Instance.GetFontFamily(this.FontFamily);
                    }
                }
                return _font;
            }
        }

        public int FontSize { get; set; } = 24;

        public string BgColor { get; set; }

        private SKColor _bgColor = SKColor.Empty;

        /// <summary>
        /// 背景色
        /// </summary>
        public SKColor SKBgColor
        {
            get
            {
                if (_bgColor == SKColor.Empty)
                {
                    if (string.IsNullOrWhiteSpace(this.BgColor))
                    {
                        _bgColor = SKColors.White;
                    }
                    else
                    {
                        _bgColor = SKColor.Parse(this.BgColor);
                    }
                }
                return _bgColor;
            }
        }

        public List<string> ForeColors { get; set; }

        private List<SKColor> _foreColors;

        /// <summary>
        /// 前景色,多个
        /// </summary>
        public List<SKColor> SKForeColors
        {
            get
            {
                if (_foreColors == null)
                {
                    _foreColors = new List<SKColor>();
                    if (this.ForeColors != null && this.ForeColors.Count > 0)
                    {
                        this.ForeColors.ForEach(color =>
                        {
                            _foreColors.Add(SKColor.Parse(color));
                        });
                    }
                    else
                    {
                        _foreColors = DefaultColors.Instance.Colors;
                    }
                }
                return _foreColors;
            }
        }

        /// <summary>
        /// 干扰气泡的配置
        /// </summary>
        public InterferenceBubbleGeneratorOption InterferenceBubble { get; set; } = new InterferenceBubbleGeneratorOption();

        /// <summary>
        /// 干扰线
        /// </summary>
        public InterferenceLineGeneratorOption InterferenceLine { get; set; } = new InterferenceLineGeneratorOption();

        public bool Animation { get; set; }

        /// <summary>
        /// 每帧时长,Animation=true时有效
        /// </summary>
        public int FrameDuration { get; set; } = 300;
    }

    public class InterferenceLineGeneratorOption
    {
        /// <summary>
        /// 生成的干扰线数量
        /// </summary>
        public int Count { get; set; } = 3;
    }

    public class InterferenceBubbleGeneratorOption
    {
        /// <summary>
        /// 生成气泡数量
        /// </summary>
        public int Count { get; set; } = 5;

        /// <summary>
        /// 气泡线宽度
        /// </summary>
        public float Thickness { get; set; } = 1;

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