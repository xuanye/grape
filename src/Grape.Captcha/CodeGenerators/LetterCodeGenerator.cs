using System.Collections.Generic;

namespace Grape.Captcha.CodeGenerators
{
    internal class LetterCodeGenerator : AbsRandomCodeGenerator
    {
        /// <summary>
        /// 生成纯字母验证码
        /// </summary>
        /// <param name="mixedCase">是否大小写混合</param>
        public LetterCodeGenerator(LetterCaseType caseType) : base(caseType) { }

        protected override List<char> InitialLetters(LetterCaseType caseType)
        {
            var chars = new List<char>();

            switch (caseType)
            {
                case LetterCaseType.Uppercase:
                    chars.AddRange(Characters.WORD_UPPER);
                    break;

                case LetterCaseType.LowerCase:
                    chars.AddRange(Characters.WORD_LOWER);
                    break;

                case LetterCaseType.MixedCase:
                default:
                    chars.AddRange(Characters.WORD_UPPER);
                    chars.AddRange(Characters.WORD_LOWER);
                    break;
            }

            return chars;
        }
    }
}