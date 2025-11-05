using Prana.Utilities.PredicateHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.PredicateHelper
{
    public class PredicateBuilderTests
    {
        [Fact]
        [Trait("Prana.Utilities", "PredicateBuilder")]
        public void And_TwoExpressions_ReturnsCombinedExpression()
        {
            // Arrange
            Expression<Func<int, bool>> expression1 = x => x > 5;
            Expression<Func<int, bool>> expression2 = x => x < 10;

            // Act
            var combinedExpression = PredicateBuilder.And(expression1, expression2);

            // Assert
            Assert.NotNull(combinedExpression);
            var compiled = combinedExpression.Compile();
            Assert.True(compiled(7)); // Both conditions are true
            Assert.False(compiled(4)); // First condition is false, second is true
            Assert.False(compiled(11)); // First condition is true, second is false
            Assert.False(compiled(3)); // First condition is false, second is true
        }

        [Fact]
        [Trait("Prana.Utilities", "PredicateBuilder")]
        public void Or_TwoExpressions_ReturnsCombinedExpression()
        {
            // Arrange
            Expression<Func<int, bool>> expression1 = x => x % 2 == 0;
            Expression<Func<int, bool>> expression2 = x => x % 3 == 0;

            // Act
            var combinedExpression = PredicateBuilder.Or(expression1, expression2);

            // Assert
            Assert.NotNull(combinedExpression);
            var compiled = combinedExpression.Compile();
            Assert.True(compiled(6)); // Both conditions are true
            Assert.True(compiled(10)); // First condition is true, second is false
            Assert.True(compiled(9)); // First condition is false, second is true
            Assert.False(compiled(7)); // Both conditions are false
        }
    }
}
