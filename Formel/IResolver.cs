using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formel
{
    public interface IResolver
    {
        decimal ResolveVariableValue(VariableToken token);
        IEnumerable<string> ProvidedVariables { get; }
    }

    public class BasicResolver : IResolver
    {
        public IEnumerable<string> ProvidedVariables => Enumerable.Empty<string>();

        public decimal ResolveVariableValue(VariableToken token)
        {
            return 0;
        }
    }

    public class DictionaryResolver : IResolver
    {
        private Dictionary<string, decimal> VariableToValueMap { get; }
        public IEnumerable<string> ProvidedVariables => VariableToValueMap.Keys;

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
