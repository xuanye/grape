using System;
using System.Collections.Generic;
using System.Text;

namespace Grape.Captcha.CodeGenerators
{
    internal abstract class AbsRandomCodeGenerator : ICaptchaCodeGenerator
    {
        private readonly List<char> _letters;
        private readonly Random _random;

        protected AbsRandomCodeGenerator() : this(LetterCaseType.MixedCase)
        {
        }

        protected AbsRandomCodeGenerator(LetterCaseType caseType)
        {
            _letters = InitialLetters(caseType);
            _random = new Random();
        }

        public (string text, string value) GenerateCode(int length)
        {
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(_letters[_random.Next(_letters.Count)]);
            }

            var text = result.ToString();
            return (text, text);
        }

        protected abstract List<char> InitialLetters(LetterCaseType caseType);
    }
}