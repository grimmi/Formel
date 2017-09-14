using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using System.Text;

namespace Formel
{
    public class Operator : IComparable<Operator>, IEquatable<Operator>
    {
        public static Operator Power = new Operator("^", Associativity.Right);
        public static Operator Times = new Operator("*", Associativity.Left);
        public static Operator Divide = new Operator("/", Associativity.Left);
        public static Operator Plus = new Operator("+", Associativity.Left);
        public static Operator Minus = new Operator("-", Associativity.Left);
        public static Operator OpenParen = new Operator("(", Associativity.Left);
        public static Operator CloseParen = new Operator(")", Associativity.Right);

        private static Dictionary<Operator, int> PrecedenceMap = new Dictionary<Operator, int>
        {
            [Power] = 4,
            [Times] = 3,
            [Divide] = 3,
            [Plus] = 2,
            [Minus] = 1,
            [OpenParen] = 0,
            [CloseParen] = 0,
        };

        public Associativity Associativity { get; set; }

        public static Operator ToOperator(string token)
        {
            return PrecedenceMap.Single(kvp => kvp.Key.Token.Equals(token)).Key;
        }

        private int GetPrecedence()
        {
            switch (Token)
            {
                case "^": return PrecedenceMap[Power];
                case "*": return PrecedenceMap[Times];
                case "/": return PrecedenceMap[Divide];
                case "+": return PrecedenceMap[Plus];
                case "-": return PrecedenceMap[Minus];
                case "(": return PrecedenceMap[OpenParen];
                case ")": return PrecedenceMap[CloseParen];
            }
            throw new ArgumentException($"unknown operator '{Token}'!");
        }

        public override string ToString()
        {
            return Token;
        }

        public string Token { get; }

        private Operator(string token, Associativity associativity)
        {
            Token = token;
            Associativity = associativity;
        }

        public decimal Operate(decimal val1, decimal val2)
        {
            switch(Token)
            {
                case "^": return (decimal)Pow((double)val1, (double)val2);
                case "*": return val1 * val2;
                case "/": return val1 / val2;
                case "+": return val1 + val2;
                case "-": return val1 - val2;
                default: throw new InvalidOperationException("this operator cannot be evaluated!");
            }
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
