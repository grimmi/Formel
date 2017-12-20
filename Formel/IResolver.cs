using System;
using System.Collections.Generic;
using System.Linq;

namespace Formel
{
    public interface IResolver
    {
        decimal ResolveVariableValue(VariableToken token);
        IEnumerable<string> ProvidedVariables { get; }
        string GetDescription(string variable);
    }

    public class BasicResolver : IResolver
    {
        public IEnumerable<string> ProvidedVariables => Enumerable.Empty<string>();

        public decimal ResolveVariableValue(VariableToken token)
        {
            return 0;
        }

        public string GetDescription(string variable) => variable;
    }

    public class DictionaryResolver : IResolver
    {
        private Dictionary<string, decimal> VariableToValueMap { get; }
        public IEnumerable<string> ProvidedVariables => VariableToValueMap.Keys;

        private Dictionary<string, string> Descriptions { get; }

        public DictionaryResolver(Dictionary<string, decimal> variableToValue, Dictionary<string, string> descriptions = null)
        {
            VariableToValueMap = variableToValue;
            Descriptions = descriptions ?? new Dictionary<string, string>();
        }

        public decimal ResolveVariableValue(VariableToken token)
        {
            if(VariableToValueMap.TryGetValue(token.OriginalValue, out decimal value))
            {
                return value;
            }

            throw new ArgumentException("unknown variable!");
        }

        public string GetDescription(string variable)
        {
            if(Descriptions.TryGetValue(variable, out string description))
            {
                return description;
            }
            return variable;
        }
    }
}
