using System.Collections.Generic;
using System.Linq;

namespace Formel
{
    public static class Formel
    {
        static string[] operators = "+,-,*,/,^,(,)".Split(new[] { ',' });

        public static IEnumerable<string> Transform(string input)
        {
            var output = new List<string>();
            var operators = new Stack<Operator>();
            var currentToken = "";
            for (int i = 0; i < input.Length; i++)
            {
                var token = input[i].ToString();
                var (isop, op) = IsOperator(token);
                if (isop)
                {
                    if (currentToken.Trim().Length > 0)
                    {
                        output.Add(currentToken.Trim());
                    }
                    currentToken = string.Empty;
                    if (op == Operator.OpenParen)
                    {
                        operators.Push(op);
                    }
                    else if (op == Operator.CloseParen)
                    {
                        var topOp = operators.Peek();
                        while (topOp != Operator.OpenParen)
                        {
                            output.Add(operators.Pop().Token);
                            topOp = operators.Peek();
                        }
                        if (topOp == Operator.OpenParen)
                        {
                            operators.Pop();
                        }
                    }
                    else if (operators.Count > 0)
                    {
                        var topOp = operators.Peek();
                        while (operators.Count > 0 && topOp.Associativity == Associativity.Left && topOp.CompareTo(op) > -1)
                        {
                            output.Add(operators.Pop().Token);
                            if (operators.Count > 0)
                            {
                                topOp = operators.Peek();
                            }
                        }
                        operators.Push(op);
                    }
                    else
                    {
                        operators.Push(op);
                    }
                }
                else
                {
                    currentToken += token.Trim();
                }
            }

            if (!string.IsNullOrWhiteSpace(currentToken))
            {
                output.Add(currentToken.Trim());
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop().Token);
            }

            return output;
        }

        internal static (bool, Operator) IsOperator(string token)
        {
            if (operators.Contains(token))
            {
                return (true, Operator.ToOperator(token));
            }
            return (false, null);
        }
    }
}
