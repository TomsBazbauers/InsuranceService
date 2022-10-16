using FluentAssertions;
using Xunit;

namespace InsuranceService
{
    public class RiskAvailabilityValidatorTests
    {
        private IRiskListValidator _sut;
        private List<Risk> _riskList;

        public RiskAvailabilityValidatorTests()
        {
            _sut = new RiskAvailabilityValidator();
            _riskList =
                new List<Risk> 
                {
                new Risk("General Insurance", 360m), new Risk("Cyber Security", 120m),
                new Risk("Burglary", 240m), new Risk("Weather Damage", 180m),
                new Risk("Arsony", 120m), 
                };
        }

        [Fact]
        public void IsValid_InputValidName_ReturnsTrue()
        {
            // Arrange
            var testRisk = new Risk("Cyber Security", 120m);

            // Act
            var actual = _sut.IsValid(testRisk, _riskList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputValidYearlyPrice_ReturnsTrue()
        {
            // Arrange
            var testRisk = new Risk("General Insurance", 360m);

            // Act
            var actual = _sut.IsValid(testRisk, _riskList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("Cyber Securities", 190m);

            // Act
            Action action = () => _sut.IsValid(testRisk, _riskList);

            // Assert
            action.Should().Throw<InvalidRiskRequestException>()
                .WithMessage($"[The requested risk offer is not found]");
        }

        [Fact]
        public void IsValid_InputInvalidYearlyPrice_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("CyberSecurity", 200);

            // Act
            Action action = () => _sut.IsValid(testRisk, _riskList);

            // Assert
            action.Should().Throw<InvalidRiskRequestException>()
                .WithMessage($"[The requested risk offer is not found]");
        }
    }
}
