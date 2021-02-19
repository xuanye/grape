using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grape.Captcha
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrapeCaptchaService(this IServiceCollection @services)
        {
            return @services.AddSingleton<ICaptchaGenerator, CaptchaGenerator>();
        }
    }
}
