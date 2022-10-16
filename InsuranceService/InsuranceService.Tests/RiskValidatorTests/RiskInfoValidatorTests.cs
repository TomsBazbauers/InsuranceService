using FluentAssertions;
using Xunit;

namespace InsuranceService
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
            var testRisk = new Risk("Cyber Security", 120m);

            // Act
            var actual = _sut.IsValid(testRisk);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputValidYearlyPrice_ReturnsTrue()
        {
            // Arrange
            var testRisk = new Risk("Cyber Security", 120m);

            // Act
            var actual = _sut.IsValid(testRisk);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("   ", 120m);

            // Act
            Action action = () => _sut.IsValid(testRisk);

            // Assert
            action.Should().Throw<InvalidRiskInfoException>()
                .WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void IsValid_InputInvalidYearlyPrice_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("Cyber Security", 0);

            // Act
            Action action = () => _sut.IsValid(testRisk);

            // Assert
            action.Should().Throw<InvalidRiskInfoException>()
                .WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }
    }
}
