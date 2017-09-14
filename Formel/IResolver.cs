using System;
using System.Collections.Generic;
using System.Text;

namespace Formel
{
    public interface IResolver
    {
        decimal ResolveVariableValue(VariableToken token);
    }

    public class BasicResolver : IResolver
    {
        public decimal ResolveVariableValue(VariableToken token)
        {
            return 0;
        }
    }

    public class DictionaryResolver : IResolver
    {
        private Dictionary<string, decimal> VariableToValueMap { get; }

        public DictionaryResolver(Dictionary<string, decimal> variableToValue)
        {
            VariableToValueMap = variableToValue;
        }

        public decimal ResolveVariableValue(VariableToken token)
        {
            if(VariableToValueMap.TryGetValue(token.OriginalValue, out decimal value))
            {
                return value;
            }

            throw new ArgumentException("unknown variable!");
        }
    }
}
