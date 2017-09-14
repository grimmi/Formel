using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Formel.Tests
{
    public class TransformerTests
    {
        [Theory]
        [InlineData("3 + 4", "3 4 +")]
        [InlineData("3 + 4 + 5", "3 4 + 5 +")]
        [InlineData("1 * 2 * 3 + 4", "1 2 * 3 * 4 +")]
        [InlineData("1 + 2 * 3 + 4", "1 2 3 * + 4 +")]
        [InlineData("1 + 2 / 3", "1 2 3 / +")]
        [InlineData("1 * ( 2 + 3 )", "1 2 3 + *")]
        [InlineData("3 + 4 * 2 / (1 - 5) ^ 2 ^ 3", "3 4 2 * 1 5 - 2 3 ^ ^ / +")]
        [InlineData("${abc} * 10 / 3", "${abc} 10 * 3 /")]
        public void Transform_ValidFormula_ReturnsTransformedOutput(string input, string expected)
        {
            var transformed = Formel.ToReversePolish(input);
            var transformedString = string.Join(" ", transformed.Select(t => t.ToString()));
            Assert.Equal(expected, transformedString);
        }
        
        [Theory]
        [InlineData("3 + 4", 7)]
        [InlineData("2 * 3", 6)]
        [InlineData("10 / 2", 5)]
        [InlineData("2 ^ 3", 8)]
        [InlineData("2 * 2 * 2", 8)]
        [InlineData("2 + 3 * 4", 14)] 
        [InlineData("2 + 2 ^ 3", 10)]
        [InlineData("(2 + 2) * 4", 16)]
        [InlineData("(2 / 4) * 2", 1)]
        [InlineData("3 + 4 * 2 / (1 - 5) ^ 2 ^ 3", 3.0001220703125)]
        public void Evaluate_ValidFormula_ReturnsCorrectResult(string input, double expected)
        {
            var transformed = Formel.ToReversePolish(input);
            var result = Formel.Evaluate(transformed);

            Assert.Equal(expected, (double)result, 15);
        }

        private IResolver GetDictionaryResolver()
        {
            return new DictionaryResolver(new Dictionary<string, decimal>
            {
                ["${abc}"] = 1,
                ["${two}"] = 2,
                ["${three.four}"] = 3.4m,
                ["${ten}"] = 10
            });
        }

        [Theory]
        [InlineData("${abc} + 2", 3)]
        [InlineData("${two} * 2", 4)]
        [InlineData("2 ^ ${ten}", 1024)]
        public void Evaluate_FormulaWithVariables_ReturnsTheCorrectResult(string input, double expected)
        {
            Formel.Resolver = GetDictionaryResolver();

            var result = Formel.Evaluate(Formel.ToReversePolish(input));

            Assert.Equal(expected, (double)result);
        }
    }
}
