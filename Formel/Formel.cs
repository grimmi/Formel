using System.Collections.Generic;
using System.Linq;

namespace Formel
{
    public static class Formel
    {
        static string[] operators = "+,-,*,/,^".Split(new[] { ',' });

        public static string Transform(string input)
        {
            var output = new List<string>();
            var operators = new PriorityQueue<Operator>();
            for (int i = 0; i < input.Length; i++)
            {
                var token = input[i].ToString();
                var (isOperator, op) = IsOperator(token);
                if (isOperator)
                {
                    if (operators.Length > 0)
                    {
                        output.Add(operators.Pop().Token);
                    }
                    operators.Push(op);
                }
                else if (IsConstant(token))
                {
                    output.Add(token);
                }
            }

            while (operators.Length > 0)
            {
                output.Add(operators.Pop().Token);
            }

            return string.Join(" ", output);
        }

        internal static bool IsConstant(string token)
        {
            return int.TryParse(token, out int value);
        }

        internal static (bool, Operator) IsOperator(string token)
        {
            if(operators.Contains(token))
            {
                return (true, Operator.ToOperator(token));
            }
            return (false, null);
        }
    }
}
