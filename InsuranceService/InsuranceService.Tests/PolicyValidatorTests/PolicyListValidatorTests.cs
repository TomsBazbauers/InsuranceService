using FluentAssertions;
using Xunit;

namespace InsuranceService
{
    public class PolicyListValidatorTests
    {
        private PolicyListValidator _sut;
        private List<IPolicy> _testList;

        public PolicyListValidatorTests()
        {
            _sut = new PolicyListValidator();
            _testList = new List<IPolicy>()
             {
                 new Policy("BMW 330 2022", new DateTime(2022, 01, 01), new DateTime(2025, 01, 01), 
                 new List<Risk>() {new Risk("General", 720m, new DateTime(2022, 01, 01)) }),
                 new Policy("AUDI A3 2020", new DateTime(2020, 01, 01), new DateTime(2020, 01, 01), 
                 new List<Risk>() {new Risk("Burglary", 320m, new DateTime(2020, 01, 01)) }),
                 new Policy("VW GOLF 2015", new DateTime(2014, 03, 19), new DateTime(2028, 01, 01), 
                 new List<Risk>() {new Risk("General", 220m, new DateTime(2014, 03, 19)) }),
                 new Policy("OPEL ZAFIRA 2021", new DateTime(2021, 04, 04), new DateTime(2030, 01, 01), 
                 new List<Risk>() {new Risk("Burglary", 320m, new DateTime(2021, 04, 04)) })
             };
        }

        [Fact]
        public void IsFound_InputValidAllProperties_ReturnsTrue()
        {
            // Arrange
            var testName = "BMW 330 2022";
            var testValidFrom = new DateTime(2022, 01, 01);

            // Act
            var actual = _sut.IsFound(testName, testValidFrom, _testList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsFound_InputValidNameUnformatted_ReturnsTrue()
        {
            // Arrange
            var testName = "opEL zAfiRA 2021";
            var testValidFrom = new DateTime(2021, 04, 04);

            // Act
            var actual = _sut.IsFound(testName, testValidFrom, _testList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsFound_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testName = "VW GOLF 2011";
            var testValidFrom = new DateTime(2022, 01, 01);

            // Act
            Action action = () => _sut.IsFound(testName, testValidFrom, _testList);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
                .WithMessage($"[Requested policy with the properties: '{string.Join(", ", testName, testValidFrom)}' is not found]");
        }

        [Fact]
        public void IsFound_InputInvalidNameEmpty_ThrowsException()
        {
            // Arrange
            var testName = "";
            var testValidFrom = new DateTime(2022, 02, 02);

            // Act
            Action action = () => _sut.IsFound(testName, testValidFrom, _testList);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
                .WithMessage($"[Requested policy with the properties: '{string.Join(", ", testName, testValidFrom)}' is not found]");
        }

        [Fact]
        public void IsFound_InputInvalidValidFrom_ThrowsException()
        {
            // Arrange
            var testName = "BMW 330 2022";
            var testValidFrom = new DateTime(2022, 02, 02);

            // Act
            Action action = () => _sut.IsFound(testName, testValidFrom, _testList);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
                .WithMessage($"[Requested policy with the properties: '{string.Join(", ", testName, testValidFrom)}' is not found]");
        }

        [Fact]
        public void IsUnique_InputValid_ReturnsTrue()
        {
            // Arrange
            var testName = "VW GOLF 2019";
            var testValidFrom = new DateTime(2018, 03, 19);

            // Act
            var actual = _sut.IsUnique(testName, testValidFrom, _testList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsUnique_InputInvalid_ThrowsException()
        {
            // Arrange
            var testName = "VW GOLF 2015";
            var testValidFrom = new DateTime(2014, 03, 19);

            // Act
            Action action = () => _sut.IsUnique(testName, testValidFrom, _testList);

            // Assert
            action.Should().Throw<DuplicatePolicyException>()
                .WithMessage($"[Policy with properties: '{string.Join(", ", testName, testValidFrom)}' is already registered in the system]");
        }
    }
}