using System.Collections.Generic;

namespace Grape.Captcha.CodeGenerators
{
    internal class ChineseNumberCodeGenerator : AbsRandomCodeGenerator
    {
        protected override List<char> InitialLetters(LetterCaseType caseType)
        {
            return Characters.NUMBER_ZH_CN;
        }
    }
}