namespace Grape.Captcha.CodeGenerators
{
    public interface ICaptchaCodeGenerator
    {
        (string text, string value) GenerateCode(int length);
    }
}