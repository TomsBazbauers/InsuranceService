using FluentAssertions;
using Xunit;

namespace InsuranceService.Tests
{
    public class PolicyInfoValidatorTests
    {
        private PolicyInfoValidator _sut;

        public PolicyInfoValidatorTests()
        {
            _sut = new PolicyInfoValidator();
        }

        [Fact]
        public void IsValid_InputValidAllProperties_ReturnsTrue()
        {
            // Arrange
            var testName = "BMW M3 2022";
            var testValidFrom = new DateTime(2022, 09, 10);
            short testDuration = 12;
            
            // Act
            var actual = _sut.IsValid(testName, testValidFrom, testDuration);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidValidMin_ThrowsException()
        {
            // Arrange
            var testName = "BMW M3 2022";
            var testValidFrom = DateTime.MinValue;
            short testDuration = 12;

            // Act
            Action action = () => _sut.IsValid(testName, testValidFrom, testDuration);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testName, testValidFrom, testDuration)}' to solve this problem]");
        }

        [Fact]
        public void IsValid_InputInvalidValidMax_ThrowsException()
        {
            // Arrange
            var testName = "BMW M3 2022";
            var testValidFrom = DateTime.Now.AddDays(10);
            short testDuration = 12;

            // Act
            Action action = () => _sut.IsValid(testName, testValidFrom, testDuration);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testName, testValidFrom, testDuration)}' to solve this problem]");
        }

        [Fact]
        public void IsValid_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testName = "     ";
            var testValidFrom = new DateTime(2022, 01, 01);
            short testDuration = 12;

            // Act
            Action action = () => _sut.IsValid(testName, testValidFrom, testDuration);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testName, testValidFrom, testDuration)}' to solve this problem]");
        }

        [Fact]
        public void IsValid_InputInvalidDuration_ThrowsException()
        {
            // Arrange
            var testName = "     ";
            var testValidFrom = new DateTime(2022, 01, 01);
            short testDuration = 0;

            // Act
            Action action = () => _sut.IsValid(testName, testValidFrom, testDuration);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testName, testValidFrom, testDuration)}' to solve this problem]");
        }
    }
}
