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
}
