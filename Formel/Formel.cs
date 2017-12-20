using System.Collections.Generic;
using System.Linq;

namespace Formel
{
    public static class Formel
    {
        public static IResolver Resolver { get; set; } = new BasicResolver();

        private static void AddTokenToList(List<Token> output, string currentToken)
        {
            if (currentToken.Trim().Length > 0)
            {
                output.Add(Token.FromString(currentToken.Trim()));
            }
        }

        private static IEnumerable<Token> PopUntilOpenParen(Stack<Operator> operators)
        {
            var topOp = operators.Peek();
            while (topOp != Operator.OpenParen)
            {
                yield return new OperatorToken(operators.Pop());
                topOp = operators.Peek();
            }
            if (topOp == Operator.OpenParen)
            {
                operators.Pop();
            }
        }

        private static IEnumerable<Token> PopHigherOperators(Stack<Operator> operators, Operator op)
        {
            var topOp = operators.Peek();
            while (operators.Count > 0 && topOp.Associativity == Associativity.Left && topOp.CompareTo(op) > -1)
            {
                yield return new OperatorToken(operators.Pop());
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
            var currentToken = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                var token = input[i].ToString();
                var (isOperator, @operator) = IsOperator(token);
                if (isOperator)
                {
                    AddTokenToList(output, currentToken);
                    switch (@operator)
                    {
                        case Operator op when op == Operator.OpenParen:
                            operators.Push(op);
                            break;
                        case Operator op when op == Operator.CloseParen:
                            output.AddRange(PopUntilOpenParen(operators));
                            break;
                        case Operator op when operators.Count > 0:
                            output.AddRange(PopHigherOperators(operators, op));
                            operators.Push(@operator);
                            break;
                        default:
                            operators.Push(@operator);
                            break;
                    }
                    currentToken = string.Empty;
                }
                else
                {
                    currentToken += token;
                }
            }

            if (!string.IsNullOrWhiteSpace(currentToken))
            {
                output.Add(Token.FromString(currentToken));
            }
            output.AddRange(operators.Select(op => new OperatorToken(op)));

            return output;
        }

        internal static (bool, Operator) IsOperator(string token)
        {
            if (Operator.IsOperatorToken(token))
            {
                return (true, Operator.ToOperator(token));
            }
            return (false, null);
        }

        public static decimal Evaluate(string formula)
        {
            return Evaluate(ToReversePolish(formula));
        }

        public static decimal Evaluate(IEnumerable<Token> formula)
        {
            var tokenList = formula.ToList();
            var valueStack = new Stack<decimal>();
            foreach(var token in tokenList)
            { 
                if (token is ConstantToken constant)
                {
                    valueStack.Push(constant.ConstantValue);
                    continue;
                }
                if (token is VariableToken variable)
                {
                    valueStack.Push(variable.Evaluate(Resolver));
                    continue;
                }
                if (token is SpaceToken) continue;
                if (token is OperatorToken opToken)
                {
                    var value2 = valueStack.Pop();
                    var value1 = valueStack.Pop();
                    valueStack.Push(opToken.Operator.Operate(value1, value2));
                }
            }

            return valueStack.Pop();
        }
    }
}
