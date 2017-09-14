using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Formel
{
    public static class Formel
    {
        static string[] operators = "+,-,*,/,^,(,)".Split(new[] { ',' });

        private static string HandleCurrentToken(List<Token> output, string currentToken)
        {
            if (currentToken.Trim().Length > 0)
            {
                output.Add(Token.FromString(currentToken.Trim()));
            }
            return string.Empty;
        }

        private static void PopUntilOpenParen(Stack<Operator> operators, List<Token> output)
        {
            var topOp = operators.Peek();
            while (topOp != Operator.OpenParen)
            {
                output.Add(new OperatorToken(operators.Pop()));
                topOp = operators.Peek();
            }
            if (topOp == Operator.OpenParen)
            {
                operators.Pop();
            }
        }

        private static void PopHigherOperators(Stack<Operator> operators, Operator op, List<Token> output)
        {
            var topOp = operators.Peek();
            while (operators.Count > 0 && topOp.Associativity == Associativity.Left && topOp.CompareTo(op) > -1)
            {
                output.Add(new OperatorToken(operators.Pop()));
                if (operators.Count > 0)
                {
                    topOp = operators.Peek();
                }
            }
        }

        private static void AppendLastToken(List<Token> output, string currentToken)
        {
            if (!string.IsNullOrWhiteSpace(currentToken))
            {
                output.Add(Token.FromString(currentToken));
            }
        }

        private static void PopAllOperatorsOntoOutput(Stack<Operator> operators, List<Token> output)
        {
            while (operators.Count > 0)
            {
                output.Add(new OperatorToken(operators.Pop()));
            }
        }

        public static IEnumerable<Token> ToReversePolish(string input)
        {
            var output = new List<Token>();
            var operators = new Stack<Operator>();
            var currentToken = "";
            for (int i = 0; i < input.Length; i++)
            {
                var token = input[i].ToString();
                var (isOperator, @operator) = IsOperator(token);
                if (isOperator)
                {
                    currentToken = HandleCurrentToken(output, currentToken);
                    switch (@operator)
                    {
                        case Operator op when op == Operator.OpenParen:
                            operators.Push(op);
                            break;
                        case Operator op when op == Operator.CloseParen:
                            PopUntilOpenParen(operators, output);
                            break;
                        case Operator op when operators.Count > 0:
                            PopHigherOperators(operators, op, output);
                            operators.Push(@operator);
                            break;
                        default:
                            operators.Push(@operator);
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

        public static decimal Evaluate(IEnumerable<Token> formula)
        {
            var tokenList = formula.ToList();
            var valueStack = new Stack<decimal>();
            for (int i = 0; i < tokenList.Count; i++)
            {
                var token = tokenList[i];
                if (token is ConstantToken constant)
                {
                    valueStack.Push(constant.ConstantValue);
                    continue;
                }
                if (token is VariableToken variable)
                {
                    valueStack.Push(variable.Evaluate(new BasicResolver()));
                    continue;
                }
                if (token is SpaceToken) continue;
                if (token is OperatorToken opToken)
                {
                    var value2 = valueStack.Pop();
                    var value1 = valueStack.Pop();                     
                    switch (opToken.Operator)
                    {
                        case Operator op when op == Operator.Plus:
                            valueStack.Push(value1 + value2);
                            break;
                        case Operator op when op == Operator.Minus:
                            valueStack.Push(value1 - value2);
                            break;
                        case Operator op when op == Operator.Times:
                            valueStack.Push((value1 * value2));
                            break;
                        case Operator op when op == Operator.Divide:
                            valueStack.Push((value1 / value2));
                            break;
                        case Operator op when op == Operator.Power:
                            valueStack.Push((decimal)Pow((double)value1, (double)value2));
                            break;
                    }
                }
            }

            return valueStack.Pop();
        }
    }
}
