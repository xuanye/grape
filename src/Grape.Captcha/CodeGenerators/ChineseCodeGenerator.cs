using System.Collections.Generic;

namespace Grape.Captcha.CodeGenerators
{
    internal class ChineseCodeGenerator : AbsRandomCodeGenerator
    {
        protected override List<char> InitialLetters(LetterCaseType caseType)
        {
            return Characters.CHINESE;
        }
    }
}