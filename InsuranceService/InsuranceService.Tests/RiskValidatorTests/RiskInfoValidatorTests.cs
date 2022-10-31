using FluentAssertions;
using Xunit;

namespace InsuranceService.Tests
{
    public class RiskInfoValidatorTests
    {
        private IRiskValidator _sut;

        public RiskInfoValidatorTests()
        {
            _sut = new RiskInfoValidator();
        }

        [Fact]
        public void IsValid_InputValidName_ReturnsTrue()
        {
            // Arrange
            var testRisk = new Risk("Cyber Security", 120m, new DateTime(2022, 01, 01));

            // Act
            var actual = _sut.IsValid(testRisk, new DateTime(2022, 01, 01));

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputValidYearlyPrice_ReturnsTrue()
        {
            // Arrange
            var testRisk = new Risk("Cyber Security", 120m, new DateTime(2022, 01, 01));

            // Act
            var actual = _sut.IsValid(testRisk, new DateTime(2022, 01, 01));

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("   ", 120m , new DateTime(2022, 01, 01));

            // Act
            Action action = () => _sut.IsValid(testRisk, new DateTime(2022, 01, 01));

            // Assert
            action.Should().Throw<InvalidRiskInfoException>()
                .WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void IsValid_InputInvalidYearlyPrice_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("Cyber Security", 0, new DateTime(2022, 01, 01));

            // Act
            Action action = () => _sut.IsValid(testRisk, new DateTime(2022, 01, 01));

            // Assert
            action.Should().Throw<InvalidRiskInfoException>()
                .WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void IsValid_InputInvalidEffectiveDate_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("Cyber Security", 120m, DateTime.MinValue);

            // Act
            Action action = () => _sut.IsValid(testRisk, new DateTime(2022, 01, 01));

            // Assert
            action.Should().Throw<InvalidRiskInfoException>()
                .WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }
    }
}
