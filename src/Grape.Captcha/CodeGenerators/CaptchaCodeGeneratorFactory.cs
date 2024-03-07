using Grape.Captcha.CodeGenerators;
using System;
using System.Collections.Generic;

namespace Grape.Captcha
{
    internal static class CaptchaCodeGeneratorFactory
    {
        private static Dictionary<CaptchaCodeGeneratorType, ICaptchaCodeGenerator> dict = new Dictionary<CaptchaCodeGeneratorType, ICaptchaCodeGenerator>()
        {
            {CaptchaCodeGeneratorType.Default, new LetterCodeGenerator(LetterCaseType.Uppercase) },
            {CaptchaCodeGeneratorType.Chinese, new ChineseCodeGenerator() },
            {CaptchaCodeGeneratorType.SChineseNumber, new ChineseNumberCodeGenerator() },
            {CaptchaCodeGeneratorType.TChineseNumber, new TraditionalChineseNumberCodeGenerator() },
            {CaptchaCodeGeneratorType.UpperCaseLetterAndNumber, new LetterNumberCodeGenerator( LetterCaseType.Uppercase) },
            {CaptchaCodeGeneratorType.LowerCaseLetterAndNumber, new LetterNumberCodeGenerator( LetterCaseType.LowerCase) },
            {CaptchaCodeGeneratorType.MixedCaseLetterAndNumber, new LetterNumberCodeGenerator( LetterCaseType.MixedCase) },
            {CaptchaCodeGeneratorType.UpperCaseLetter, new LetterCodeGenerator(LetterCaseType.Uppercase) },
            {CaptchaCodeGeneratorType.LowerCaseLetter, new LetterCodeGenerator(LetterCaseType.LowerCase) },
            {CaptchaCodeGeneratorType.MixedCaseLetter, new LetterCodeGenerator(LetterCaseType.MixedCase) },
            {CaptchaCodeGeneratorType.Arithmetic, new ArithmeticCodeGenerator() },
            {CaptchaCodeGeneratorType.ArithmeticWithChineseLetter, new ArithmeticChineseCodeGenerator() },
        };

        public static ICaptchaCodeGenerator GetCaptchaCodeGenerator(CaptchaCodeGeneratorType type)
        {
            if (dict.TryGetValue(type, out var generator))
            {
                return generator;
            }
            throw new InvalidOperationException("unsupported CaptchaCodeGeneratorType");
        }
    }

    public enum CaptchaCodeGeneratorType
    {
        Default = 0, //大写字母
        Chinese = 1, //中文文字
        Number = 2, //数字
        SChineseNumber = 3, //中文数字
        TChineseNumber = 4, //繁体中文数字
        UpperCaseLetterAndNumber = 5, //数字+大写字母
        LowerCaseLetterAndNumber = 6, //数字+小写字母

        MixedCaseLetterAndNumber = 7, //大小字母+数字

        UpperCaseLetter = 8, //大写字母
        LowerCaseLetter = 9, //小写字母
        MixedCaseLetter = 10, //大小写字母

        Arithmetic = 11, //算术表达式
        ArithmeticWithChineseLetter = 12, //中文算术表达式
    }

    public enum LetterCaseType
    {
        LowerCase = 0,
        Uppercase = 1,
        MixedCase = 2,
    }
}