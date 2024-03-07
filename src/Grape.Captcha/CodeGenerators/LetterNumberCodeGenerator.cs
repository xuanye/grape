using System.Collections.Generic;

namespace Grape.Captcha.CodeGenerators
{
    internal class LetterNumberCodeGenerator : AbsRandomCodeGenerator
    {
        public LetterNumberCodeGenerator(LetterCaseType caseType) : base(caseType)
        {
        }

        protected override List<char> InitialLetters(LetterCaseType caseType)
        {
            var chars = new List<char>();
            chars.AddRange(Characters.NUMBER);

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