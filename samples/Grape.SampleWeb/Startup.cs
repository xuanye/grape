using Grape.Captcha;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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

                    var captchaCode = Guid.NewGuid().ToString("D");
                    var captchaResult = await captchaGenerator.GenerateCaptchaAsync(captchaCode);

                    context.Response.ContentType = "image/jpeg";
                    var buf = captchaResult.Data;
                    await context.Response.Body.WriteAsync(buf, 0, buf.Length);
                    await context.Response.Body.FlushAsync();
                    //await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}