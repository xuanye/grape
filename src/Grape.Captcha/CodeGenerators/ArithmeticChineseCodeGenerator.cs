using System.Collections.Generic;
using System.Text;

namespace Grape.Captcha.CodeGenerators
{
    internal class ArithmeticChineseCodeGenerator : ArithmeticCodeGenerator
    {
        private readonly Dictionary<char, char> _charMap = new Dictionary<char, char>();

        public ArithmeticChineseCodeGenerator()
        {
            for (int i = 0; i < Characters.NUMBER.Count; i++)
            {
                _charMap.Add(Characters.NUMBER[i], Characters.NUMBER_ZH_CN[i]);
            }

            _charMap.Add('=', '=');
            _charMap.Add('?', '?');
            _charMap.Add('+', '加');
            _charMap.Add('-', '减');
            _charMap.Add('x', '乘');
        }

        public override (string text, string value) GenerateCode(int length)
        {
            var (text, value) = base.GenerateCode(length);
            var sb = new StringBuilder(length);
            foreach (var c in text)
            {
                if (_charMap.TryGetValue(c, out var mapChar))
                {
                    sb.Append(mapChar);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return (sb.ToString(), value);
        }
    }
}