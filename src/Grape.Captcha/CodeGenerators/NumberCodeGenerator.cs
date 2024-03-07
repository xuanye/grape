using System.Collections.Generic;

namespace Grape.Captcha.CodeGenerators
{
    internal class NumberCodeGenerator : AbsRandomCodeGenerator
    {
        protected override List<char> InitialLetters(LetterCaseType caseType)
        {
            return Characters.NUMBER;
        }
    }
}