using System;
using System.Collections.Generic;

namespace Formel
{
    public class Operator : IComparable<Operator>, IEquatable<Operator>
    {
        private static string PowerToken = "^";
        private static string TimesToken = "*";
        private static string DivideToken = "/";
        private static string PlusToken = "+";
        private static string MinusToken = "-";
        private static string OpenParenToken = "(";
        private static string CloseParenToken = ")";

        public static Operator Power => TokenToOperatorMap[PowerToken];
        public static Operator Times => TokenToOperatorMap[TimesToken];
        public static Operator Divide => TokenToOperatorMap[DivideToken];
        public static Operator Plus => TokenToOperatorMap[PlusToken];
        public static Operator Minus => TokenToOperatorMap[MinusToken];
        public static Operator OpenParen => TokenToOperatorMap[OpenParenToken];
        public static Operator CloseParen => TokenToOperatorMap[CloseParenToken];

        private static Dictionary<string, Operator> TokenToOperatorMap = new Dictionary<string, Operator>
        {
            [PowerToken] = new Operator(PowerToken, Associativity.Right) { Operation = (v1, v2) => (decimal)Math.Pow((double)v1, (double)v2) },
            [TimesToken] = new Operator(TimesToken) { Operation = (v1, v2) => v1 * v2 },
            [DivideToken] = new Operator(DivideToken) { Operation = (v1, v2) => v1 / v2 },
            [PlusToken] = new Operator(PlusToken) { Operation = (v1, v2) => v1 + v2 },
            [MinusToken] = new Operator(MinusToken) { Operation = (v1, v2) => v1 - v2 },
            [OpenParenToken] = new Operator(OpenParenToken),
            [CloseParenToken] = new Operator(CloseParenToken, Associativity.Right)
        };

        private static Dictionary<string, int> PrecedenceMap = new Dictionary<string, int>
        {
            [PowerToken] = 4,
            [TimesToken] = 3,
            [DivideToken] = 3,
            [PlusToken] = 2,
            [MinusToken] = 1,
            [OpenParenToken] = 0,
            [CloseParenToken] = 0,
        };

        private Func<decimal, decimal, decimal> Operation { get; set; }

        public Associativity Associativity { get; set; }

        public static bool IsOperatorToken(string token) => PrecedenceMap.ContainsKey(token);

        public static void AddOperator(string token, Associativity associativity, int precedence, Func<decimal, decimal, decimal> operation)
        {
            var newOp = new Operator(token, associativity)
            {
                Operation = operation
            };
            TokenToOperatorMap[token] = newOp;
            PrecedenceMap[token] = precedence;
        }

        public static Operator ToOperator(string token)
        {
            return TokenToOperatorMap[token];
        }

        private int GetPrecedence() => PrecedenceMap[Token];

        public override string ToString()
        {
            return Token;
        }

        public string Token { get; }
        
        private Operator(string token, Associativity associativity = Associativity.Left)
        {
            Token = token;
            Associativity = associativity;
        }

        public decimal Operate(decimal val1, decimal val2)
        {
            if(Operation != null)
            {
                return Operation(val1, val2);
            }
            throw new InvalidOperationException("this operator cannot be evaluated!");
        }

        public int CompareTo(Operator other)
        {
            return GetPrecedence().CompareTo(other.GetPrecedence());
        }

        public override bool Equals(object obj)
        {
            if (obj is Operator other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(Operator other)
        {
            return Token.Equals(other.Token);
        }

        public override int GetHashCode()
        {
            return Token.GetHashCode();
        }
    }
}
