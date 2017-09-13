using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Formel.Tests
{
    public class TransformerTests
    {
        [Fact]
        public void Transform_ValidFormula_ReturnsTransformedOutput()
        {
            var input = "3 + 4";

            var transformed = Formel.Transform(input);

            Assert.Equal("3 4 +", transformed);
        }
    }
}
