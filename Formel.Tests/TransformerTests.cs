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
        
    }
}
