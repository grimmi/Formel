using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formel
{
    public class Operator : IComparable<Operator>, IEquatable<Operator>
    {
        public static Operator Power = new Operator("^");
        public static Operator Times = new Operator("*");
        public static Operator Divide = new Operator("/");
        public static Operator Plus = new Operator("+");
        public static Operator Minus = new Operator("-");

        private static Dictionary<Operator, int> PrecedenceMap = new Dictionary<Operator, int>
        {
            [Power] = 4,
            [Times] = 3,
            [Divide] = 3,
            [Plus] = 2,
            [Minus] = 1
        };

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
            }
            throw new ArgumentException($"unknown operator '{Token}'!");
        }

        public override string ToString()
        {
            return Token;
        }

        public string Token { get; }

        private Operator(string token) { Token = token; }

        public int CompareTo(Operator other)
        {
            return GetPrecedence().CompareTo(other.GetPrecedence());
        }

        public override bool Equals(object obj)
        {
            if(obj is Operator other)
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
