using FluentAssertions;
using Xunit;

namespace InsuranceService.Tests
{
    public class PremiumCalculatorTests
    {
        [Fact]
        public void CalculatePremium_InputClean_ReturnsExpectedAmount()
        {
            // Arrange
            var testValidFrom = new DateTime(2022, 01, 01);
            var testValidTill = new DateTime(2024, 01, 01);
            var testInsuredRisks = new List<Risk>() 
            { 
                new Risk("General", 360m, new DateTime(2022, 01, 01)), 
                new Risk("Burglary", 720m, new DateTime(2022, 01, 01)) 
            };
            var expected = (360 + 720) / 12 * 24;

            // Act
            var actual = new PremiumCalculator(testValidFrom, testValidTill, testInsuredRisks);

            // Assert
            actual.TotalPayable.Should().Be(expected);
        }

        [Fact]
        public void CalculatePremium_InputDirty_ReturnsExpectedAmount()
        {
            // Arrange
            var testValidFrom = new DateTime(2022, 01, 01);
            var testValidTill = new DateTime(2024, 02, 15);
            var testInsuredRisks = new List<Risk>() 
            {
                new Risk("General", 360m, new DateTime(2022, 01, 12)),
                new Risk("Burglary", 720m, new DateTime(2023, 11, 01)) 
            };
            var expected = 930m;

            // Act
            var actual = new PremiumCalculator(testValidFrom, testValidTill, testInsuredRisks);

            // Assert
            actual.TotalPayable.Should().Be(expected);
        }
    }
}
