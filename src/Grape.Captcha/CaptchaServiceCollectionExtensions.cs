using Grape.Captcha;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CaptchaServiceCollectionExtensions
    {
        public static IServiceCollection AddGrapeCaptchaService(this IServiceCollection services, Action<CaptchaOptions> optionsAction = null)
        {
            services.AddSingleton(p =>
            {
                var optionsAccessor = p.GetService<IOptions<CaptchaOptions>>();
                var options = optionsAccessor.Value ?? new CaptchaOptions();
                optionsAction?.Invoke(options);
                return options;
            });

            services.AddSingleton<ICaptchaGenerator, DefaultCaptchaGenerator>();

            return services;
        }
    }
}