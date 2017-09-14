using System.Collections.Generic;
using System.Linq;

namespace Formel
{
    public static class Formel
    {
        static string[] operators = "+,-,*,/,^,(,)".Split(new[] { ',' });

        private static string HandleCurrentToken(List<string> output, string currentToken)
        {
            if (currentToken.Trim().Length > 0)
            {
                output.Add(currentToken.Trim());
            }
            return string.Empty;
        }

        private static void PopUntilOpenParen(Stack<Operator> operators, List<string> output)
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

        private static void PopHigherOperators(Stack<Operator> operators, Operator op, List<string> output)
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
        }

        private static void AppendLastToken(List<string> output, string currentToken)
        {
            if (!string.IsNullOrWhiteSpace(currentToken))
            {
                output.Add(currentToken.Trim());
            }
        }

        private static void PopAllOperatorsOntoOutput(Stack<Operator> operators, List<string> output)
        {
            while (operators.Count > 0)
            {
                output.Add(operators.Pop().Token);
            }
        }

        public static IEnumerable<string> ToReversePolish(string input)
        {
            var output = new List<string>();
            var operators = new Stack<Operator>();
            var currentToken = "";
            for (int i = 0; i < input.Length; i++)
            {
                var token = input[i].ToString();
                var (isop, tokenOp) = IsOperator(token);
                if (isop)
                {
                    currentToken = HandleCurrentToken(output, currentToken);
                    switch(tokenOp)
                    {
                        case Operator op when op == Operator.OpenParen:
                            operators.Push(op);
                            break;
                        case Operator op when op == Operator.CloseParen:
                            PopUntilOpenParen(operators, output);
                            break;
                        case Operator op when operators.Count > 0:
                            PopHigherOperators(operators, op, output);
                            operators.Push(tokenOp);
                            break;
                        default:
                            operators.Push(tokenOp);
                            break;                        
                    }
                }
                else
                {
                    currentToken += token;
                }
            }

            AppendLastToken(output, currentToken);
            PopAllOperatorsOntoOutput(operators, output);

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
