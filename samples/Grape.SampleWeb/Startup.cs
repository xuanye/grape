using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grape.Captcha;

namespace Grape.SampleWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加验证码服务
            services.AddGrapeCaptchaService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/captcha", async context =>
                {
                    var captchaGenerator = app.ApplicationServices.GetRequiredService<ICaptchaGenerator>();

                    int length = 4;

                    var qL = context.Request.Query["length"];
                    if (int.TryParse(qL, out var intQL))
                    {
                        length = intQL;
                    }

                    var height = 30;
                    string captchaCode = await captchaGenerator.GenerateRandomCaptchaAsync(length);
                    int width = Convert.ToInt32(Math.Round(height * 0.6 * captchaCode.Length, 0));
                    var captchaResult = await captchaGenerator.GenerateCaptchaImageAsync(captchaCode, width, height);

                    context.Response.ContentType = "image/jpeg";
                    var buf = captchaResult.CaptchaMemoryStream.ToArray();
                    await context.Response.Body.WriteAsync(buf, 0, buf.Length);
                    await context.Response.Body.FlushAsync();
                    //await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
