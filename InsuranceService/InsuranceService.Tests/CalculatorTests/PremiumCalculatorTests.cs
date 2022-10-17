using FluentAssertions;
using Xunit;

namespace InsuranceService
{
    public class PremiumCalculatorTests
    {
        [Fact]
        public void CalculatePremium()
        {
            // Arrange
            var testValidFrom = new DateTime(2022, 01, 01);
            var testValidTill = new DateTime(2024, 01, 01);
            var testInsuredRisks = new List<Risk>() { new Risk("General", 360m, new DateTime(2022, 01, 01)), new Risk("Burglary", 720m, new DateTime(2022, 01, 01)) };
            var expected = (360 + 720) / 12 * 24;

            // Act
            var actual = new PremiumCalculator(testValidFrom, testValidTill, testInsuredRisks);

            // Assert
            actual.TotalPayable.Should().Be(expected);
        }
    }
}