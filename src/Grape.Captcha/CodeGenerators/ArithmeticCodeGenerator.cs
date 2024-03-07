using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grape.Captcha.CodeGenerators
{
    internal class ArithmeticCodeGenerator : ICaptchaCodeGenerator
    {
        private static readonly EvaluationEngine evaluator = new EvaluationEngine();

        private readonly Random _random = new Random();

        public virtual (string text, string value) GenerateCode(int length)
        {
            (var operands, var operators) = GenerateaArithmeticParts(length);

            // 生成表达式
            var sb = new StringBuilder();
            sb.Append(operands[0]);
            for (var i = 0; i < length - 1; i++)
            {
                sb.Append(operators[i]);
                sb.Append(operands[i + 1]);
            }

            var result = ((int)evaluator.Evaluate(sb.ToString())).ToString();
            return (sb.Append("=?").ToString(), result);
        }

        /// <summary>
        /// 生成算术表达式组成部分
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected (List<int> operands, List<char> operators) GenerateaArithmeticParts(int length)
        {
            // 生成操作数
            var operands = new List<int>();
            Enumerable.Range(0, length).ToList().ForEach(x => operands.Add(_random.Next(10)));

            // 生成操作符
            var operators = new List<char>();
            Enumerable.Range(0, length - 1).ToList().ForEach(x =>
            {
                switch (_random.Next(3))
                {
                    case 0:
                        operators.Add('+');
                        break;

                    case 1:
                        operators.Add('-');
                        break;

                    case 2:
                        operators.Add('x');
                        break;
                }
            });

            // 在数字运算的时候出现减法，建议结果不要出现负数
            // 多操作数比较复杂，目前仅修复两个操作数的情况
            if (length == 2 && operators[0] == '-')
            {
                var max = Math.Max(operands[0], operands[1]);
                var min = Math.Min(operands[0], operands[1]);
                operands[0] = max;
                operands[1] = min;
            }

            return (operands, operators);
        }
    }
}