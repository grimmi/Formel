using System.Collections.Generic;
using System.Linq;
using static System.Math;
using Xunit;

namespace Formel.Tests
{
    public class FormelTests
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
        public void ToReversePolish_ValidInput_ReturnsFormulaInReversePolishNotation(string input, string expected)
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
            var result = Formel.Evaluate(input);

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
        [InlineData("(${ten} + ${two}) * 2", 24)]
        public void Evaluate_FormulaWithVariables_ReturnsTheCorrectResult(string input, double expected)
        {
            Formel.Resolver = GetDictionaryResolver();

            var result = Formel.Evaluate(input);

            Assert.Equal(expected, (double)result);
        }

        [Fact]
        public void AddedModuloOperator_ValidFormula_ShouldEvaluateCorrectly()
        {
            Operator.AddOperator("%", Associativity.Left, 3, (v1, v2) => v1 % v2);

            var result = Formel.Evaluate("10 % 3");
            Assert.Equal(1, result);
        }

        [Fact]
        public void AddedLogOperator_ValidFormula_ShouldEvalueCorrectly()
        {
            Operator.AddOperator("#", Associativity.Left, 3, (v1, v2) => (decimal)Log((double)v1, (double)v2));

            var result = Formel.Evaluate("10 # 2");
            Assert.Equal(3.32192809m, result, 8);
        }
    }
}
