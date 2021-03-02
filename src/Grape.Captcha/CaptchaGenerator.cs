using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grape.Captcha
{
    public class CaptchaGenerator : ICaptchaGenerator
    {

        public CaptchaGenerator(IOptions<CaptchaGeneratorOptions> options = null)
        {
            if (options != null && options.Value != null)
            {
                this._captchaFontFamily = options.Value.CaptchaFontFamily;
            }
            else
            {
                var fontStream = this.GetType().Assembly.GetManifestResourceStream("Grape.Captcha.font.font.ttf");
                this._captchaFontFamily = LoadFontFamily(fontStream);
            }
          
        }
        const string Letters = "2346789ABCDEFGHJKLMNPRTUVWXYZ";
        //const string Letters = "0123456789";

        private FontFamily _captchaFontFamily;




        public Task<CaptchaResult> GenerateCaptchaImageAsync(string captchaCode, int width = 0, int height = 30)
        {
            if (width <= 0)
            {
                width = Convert.ToInt32(Math.Round(height * 0.8 * captchaCode.Length, 0));
            }
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Random rand = new Random();


                //使用亮色填充
                graph.Clear(GetRandomLightColor(rand));
                //使用白色填充
                //graph.Clear(Color.White);

                //绘制验证码
                DrawCaptchaCode(graph, rand, captchaCode, width, height);
                //绘制干扰线
                DrawDisorderLine(graph, rand, width, height);

                //字体样式偏移
                AdjustRippleEffect(baseMap);

                //保存图片到内存中
                MemoryStream ms = new MemoryStream();
                baseMap.Save(ms, ImageFormat.Jpeg);
                return Task.FromResult(new CaptchaResult { CaptchaCode = captchaCode, CaptchaMemoryStream = ms, Timestamp = DateTime.Now });

            }
        }


        public Task<string> GenerateRandomCaptchaAsync(int codeLength = 4)
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < codeLength; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return Task.FromResult(sb.ToString());
        }

        #region 私有方法



        private static FontFamily LoadFontFamily(string fileName)
        {
            using (var pfc = new PrivateFontCollection())
            {
                pfc.AddFontFile(fileName);
                return pfc.Families[0];
            }
        }
        // Load font family from stream
        private static FontFamily LoadFontFamily(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return LoadFontFamily(buffer);
        }
        // load font family from byte array
        private static FontFamily LoadFontFamily(byte[] buffer)
        {
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                using (var pvc = new PrivateFontCollection())
                {
                    pvc.AddMemoryFont(ptr, buffer.Length);
                    return pvc.Families[0];
                }
            }
            finally { handle.Free(); }
        }

        private void DrawCaptchaCode(Graphics graph, Random rand, string captchaCode, int width, int height)
        {
            var fontBrush = new SolidBrush(Color.Black);
            var fontSize = GetFontSize(width, captchaCode.Length);
            //Console.WriteLine("🚀 ~ file: CaptchaGenerator.cs ~ line 117 ~ fontSize={0}", fontSize);

            var fontFamily = this._captchaFontFamily ?? FontFamily.GenericSerif;
            using (var font = new Font(fontFamily, fontSize + 4, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                for (var i = 0; i < captchaCode.Length; i++)
                {
                    fontBrush.Color = GetRandomDeepColor(rand);

                    var shiftPx = fontSize / 6;

                    var x = i * fontSize * 1.0f + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                    var maxY = height - fontSize;
                    if (maxY < 0) maxY = 0;
                    float y = 0;//rand.Next(0, maxY);

                    graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                }
            }

        }

        /// <summary>
        /// 获取每个字符平均的宽度
        /// </summary>
        /// <param name="imageWidth"></param>
        /// <param name="captchaCodeCount"></param>
        /// <returns></returns>
        private static int GetFontSize(int imageWidth, int captchaCodeCount)
        {
            var averageSize = imageWidth / captchaCodeCount;

            return Convert.ToInt32(averageSize);
        }

        /// <summary>
        /// 获取随机的深色颜色
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private static Color GetRandomDeepColor(Random rand)
        {
            const int redLow = 160;
            const int greenLow = 100;
            const int blueLow = 160;
            return Color.FromArgb(rand.Next(redLow), rand.Next(greenLow), rand.Next(blueLow));
        }

        /// <summary>
        /// 获取随机的高亮颜色
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private static Color GetRandomLightColor(Random rand)
        {
            const int low = 180;
            const int high = 255;

            var nRend = rand.Next(high) % (high - low) + low;
            var nGreen = rand.Next(high) % (high - low) + low;
            var nBlue = rand.Next(high) % (high - low) + low;

            return Color.FromArgb(nRend, nGreen, nBlue);
        }

        /// <summary>
        /// 绘制干扰线
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="rand"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static void DrawDisorderLine(Graphics graph, Random rand, int width, int height)
        {
            var linePen = new Pen(new SolidBrush(Color.Black), 3);
            for (var i = 0; i < rand.Next(3, 5); i++)
            {
                linePen.Color = GetRandomDeepColor(rand);

                var startPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                var endPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                graph.DrawLine(linePen, startPoint, endPoint);

                //Point bezierPoint1 = new Point(rand.Next(0, width), rand.Next(0, height));
                //Point bezierPoint2 = new Point(rand.Next(0, width), rand.Next(0, height));

                //graph.DrawBezier(linePen, startPoint, bezierPoint1, bezierPoint2, endPoint);
            }
        }

        /// <summary>
        /// 修改bitmap 做字体样式偏移
        /// </summary>
        /// <param name="baseMap"></param>
        private static void AdjustRippleEffect(Bitmap baseMap)
        {
            const short nWave = 6;
            var nWidth = baseMap.Width;
            var nHeight = baseMap.Height;

            var pt = new Point[nWidth, nHeight];

            for (var x = 0; x < nWidth; ++x)
            {
                for (var y = 0; y < nHeight; ++y)
                {
                    var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                    var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                    var newX = x + xo;
                    var newY = y + yo;

                    if (newX > 0 && newX < nWidth)
                    {
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        pt[x, y].X = 0;
                    }


                    if (newY > 0 && newY < nHeight)
                    {
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        pt[x, y].Y = 0;
                    }
                }
            }

            var bSrc = (Bitmap)baseMap.Clone();

            var bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            var scanline = bitmapData.Stride;

            var scan0 = bitmapData.Scan0;
            var srcScan0 = bmSrc.Scan0;

            unsafe
            {
                var p = (byte*)(void*)scan0;
                var pSrc = (byte*)(void*)srcScan0;

                var nOffset = bitmapData.Stride - baseMap.Width * 3;

                for (var y = 0; y < nHeight; ++y)
                {
                    for (var x = 0; x < nWidth; ++x)
                    {
                        var xOffset = pt[x, y].X;
                        var yOffset = pt[x, y].Y;

                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            if (pSrc != null)
                            {
                                p[0] = pSrc[yOffset * scanline + xOffset * 3];
                                p[1] = pSrc[yOffset * scanline + xOffset * 3 + 1];
                                p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
                            }
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            baseMap.UnlockBits(bitmapData);
            bSrc.UnlockBits(bmSrc);
            bSrc.Dispose();
        }
        #endregion
    }
}
