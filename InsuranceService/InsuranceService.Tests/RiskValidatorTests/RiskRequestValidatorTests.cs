using FluentAssertions;
using Xunit;

namespace InsuranceService.Tests    
{
    public class RiskRequestValidatorTests
    {
        private IRiskListValidator _sut;
        private List<Risk> _riskList;

        public RiskRequestValidatorTests()
        {
            _sut = new RiskRequestValidator();
            _riskList = new List<Risk> 
            { 
                new Risk("General Insurance", 20m, new DateTime(2022, 01, 01)), 
                new Risk("Cyber Security", 100m, new DateTime(2022, 01, 01)), 
                new Risk("Burglary", 250m, new DateTime(2022, 01, 01)), 
                new Risk("Weather Damage", 400m, new DateTime(2022, 01, 01)) 
            };
        }

        [Theory]
        [InlineData(20, 100, 250, 400)]
        public void IsValid_InputValidName_ReturnsTrue(decimal caseA, decimal caseB, decimal caseC, decimal caseD)
        {
            // Act
            var actualA= _sut.IsValid(new Risk("General Insurance", caseA, new DateTime(2022, 01, 01)), _riskList);
            var actualB = _sut.IsValid(new Risk("Cyber Security", caseB, new DateTime(2022, 01, 01)), _riskList);
            var actualC = _sut.IsValid(new Risk("Burglary", caseC, new DateTime(2022, 01, 01)), _riskList);
            var actualD = _sut.IsValid(new Risk("Weather Damage", caseD, new DateTime(2022, 01, 01)), _riskList);

            // Assert
            actualA.Should().BeTrue();
            actualB.Should().BeTrue();
            actualC.Should().BeTrue();
            actualD.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputValidYearlyPrice_ReturnsTrue()
        {
            // Arrange
            var testRisk = new Risk("General Insurance", 20m, new DateTime(2022, 01, 01));

            // Act
            var actual = _sut.IsValid(testRisk, _riskList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testRisk = new Risk("Cyber Securities", 190m, new DateTime(2022, 01, 01));

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
            var testRisk = new Risk("CyberSecurity", 200, new DateTime(2022, 01, 01));

            // Act
            Action action = () => _sut.IsValid(testRisk, _riskList);

            // Assert
            action.Should().Throw<InvalidRiskRequestException>()
               .WithMessage($"[The requested risk offer is not found]");
        }
    }
}