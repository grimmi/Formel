using System;
using System.Collections.Generic;
using System.Text;

namespace Formel
{
    public enum TokenType
    {
        Variable,
        Constant,
        Operator,
        Space
    }

    public abstract class Token
    {
        public TokenType Type { get; }
        public string OriginalValue { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            OriginalValue = value;
        }

        public static Token FromString(string input)
        {
            input = input.Trim();

            if(string.IsNullOrWhiteSpace(input))
            {
                return new SpaceToken(input);
            }
            else if(int.TryParse(input, out int i))
            {
                return new ConstantToken(input);
            }
            else if(input.StartsWith("${") && input.EndsWith("}"))
            {
                return new VariableToken(input);
            }
            else
            {
                return new OperatorToken(input);
            }
        }
    }

    public class SpaceToken : Token
    {
        public SpaceToken(string value) : base(TokenType.Space, value)
        {
        }

        public override string ToString()
        {
            return " ";
        }
    }

    public class ConstantToken : Token
    {
        public ConstantToken(string value) : base(TokenType.Constant, value)
        {
        }

        public decimal ConstantValue => decimal.Parse(OriginalValue);

        public override string ToString()
        {
            return ConstantValue.ToString();
        }
    }

    public class OperatorToken : Token
    {
        public Operator Operator { get; }

        public OperatorToken(string value) : base(TokenType.Operator, value)
        {
            Operator = Operator.ToOperator(value);
        }

        public OperatorToken(Operator op) : this(op.Token)
        {
            Operator = op;
        }

        public override string ToString()
        {
            return Operator.ToString();
        }
    }

    public class VariableToken : Token
    {
        public VariableToken(string value) : base(TokenType.Variable, value)
        {
        }

        public decimal Evaluate(IResolver resolver)
        {
            return resolver.ResolveVariableValue(this);
        }

        public override string ToString()
        {
            return OriginalValue;
        }
    }
}
