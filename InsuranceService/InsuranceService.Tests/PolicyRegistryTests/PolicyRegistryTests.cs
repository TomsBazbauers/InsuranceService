using FluentAssertions;
using Xunit;

namespace InsuranceService
{
    public class PolicyRegistryTests
    {
        private IPolicyRegistry _sut;
        private IList<IPolicy> _registeredPolicies;
        private IEnumerable<IPolicyValidator> _validators;
        private IPolicyListValidator _listValidator;

        public PolicyRegistryTests()
        {
            _listValidator = new PolicyListValidator();
            _validators = new List<IPolicyValidator>() { new PolicyInfoValidator() };
            _registeredPolicies = new List<IPolicy>()
             {
                 new Policy("BMW 330 2022", 
                 new DateTime(2022, 01, 01), new DateTime(2025, 01, 01), 
                 new List<Risk>() {new Risk("General", 720m, new DateTime(2022, 01, 01)) }),
                 new Policy("AUDI A3 2020", 
                 new DateTime(2023, 01, 01), new DateTime(2025, 01, 01), 
                 new List<Risk>() {new Risk("Burglary", 320m, new DateTime(2023, 01, 01)) }),
                 new Policy("VW GOLF 2015", 
                 new DateTime(2014, 03, 19), new DateTime(2028, 01, 01), 
                 new List<Risk>() {new Risk("General", 220m, new DateTime(2014, 03, 19)) }),
                 new Policy("OPEL ZAFIRA 2021", 
                 new DateTime(2021, 04, 04), new DateTime(2030, 01, 01), 
                 new List<Risk>() {new Risk("Burglary", 320m, new DateTime(2021, 04, 04)) })
             };

            _sut = new PolicyRegistry(_registeredPolicies, _validators, _listValidator);
        }

        [Fact]
        public void RegisterPolicy_InputValidAllProperties_ReturnsPolicy()
        {
            // Arrange
            var testName = "AUDI A3 2022";
            var testValidFrom = new DateTime(2022, 01, 01);
            short testDuration = 36;
            var testRisks = new List<Risk>() { 
                new Risk("Burglary", 320m, new DateTime(2022, 01, 01)), 
                new Risk("General", 220m, new DateTime(2022, 01, 01)) };
            var testValidTill = testValidFrom.AddMonths(testDuration);
            var currentListCount = _registeredPolicies.Count;

            // Act
            var actual = _sut.RegisterPolicy(testName, testValidFrom, testDuration, testRisks);

            // Assert
            actual.NameOfInsuredObject.Should().Be(testName);
            actual.ValidFrom.Should().Be(testValidFrom);
            actual.ValidTill.Should().Be(testValidTill);
            actual.InsuredRisks.Should().BeSameAs(testRisks);
            actual.Premium.Should().Be(1620);
            _registeredPolicies.Count.Should().Be(currentListCount + 1);
        }

        [Fact]
        public void RegisterPolicy_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testName = "";
            var testValidFrom = new DateTime(2022, 05, 01);
            short testDuration = 36;
            var testRisks = new List<Risk>() { 
                new Risk("Burglary", 320m, new DateTime(2023, 01, 01)), 
                new Risk("General", 220m, new DateTime(2023, 01, 01)) };

            // Act
            Action action = () => _sut.RegisterPolicy(testName, testValidFrom, testDuration, testRisks);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testName, testValidFrom, testDuration)}' to solve this problem]");
        }

        [Fact]
        public void RegisterPolicy_InputInvalidDate_ThrowsException()
        {
            // Arrange
            var testName = "MERCEDES-BENZ E400 2010";
            var testValidFrom = DateTime.Now.AddDays(14);
            short testDuration = 36;
            var testRisks = new List<Risk>() { 
                new Risk("Burglary", 320m, new DateTime(2023, 01, 01)), 
                new Risk("General", 220m, new DateTime(2023, 01, 01)) };

            // Act
            Action action = () => _sut.RegisterPolicy(testName, testValidFrom, testDuration, testRisks);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testName, testValidFrom, testDuration)}' to solve this problem]");
        }

        [Fact]
        public void RegisterPolicy_InputInvalidDuration_ThrowsException()
        {
            // Arrange
            var testName = "SKODA FABIA 2022";
            var testValidFrom = new DateTime(2022, 05, 01);
            short testDuration = 0;
            var testRisks = new List<Risk>() { 
                new Risk("Burglary", 320m, new DateTime(2022, 05, 01)), 
                new Risk("General", 220m, new DateTime(2022, 05, 01)) };

            // Act
            Action action = () => _sut.RegisterPolicy(testName, testValidFrom, testDuration, testRisks);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testName, testValidFrom, testDuration)}' to solve this problem]");
        }

        [Fact]
        public void AddCoverage_InputValid_ReturnsTrue()
        {
            // Arrange
            var testPolicy = _registeredPolicies[0];
            var testRisk = new Risk("Burglary", 220m, new DateTime(2022, 01, 01));

            // Act
            _sut.AddCoverage(testPolicy.NameOfInsuredObject, testRisk, testPolicy.ValidFrom);

            // Assert
            testPolicy.InsuredRisks.Count.Should().Be(2);
            testPolicy.InsuredRisks[1].Name.Should().Be(testRisk.Name);
            testPolicy.InsuredRisks[1].YearlyPrice.Should().Be(testRisk.YearlyPrice);
        }

        [Fact]
        public void AddCoverage_InputInvalid_ThrowsException()
        {
            // Arrange
            var testPolicy = new Policy("BMW 530 2020",
                new DateTime(2022, 01, 01),
                new DateTime(2025, 01, 01),
                new List<Risk>() { new Risk("General", 720m, new DateTime(2022, 01, 01)) });
            var testRisk = new Risk("Burglary", 220m, new DateTime(2022, 01, 01));

            // Act
            Action action = () => _sut.AddCoverage(testPolicy.NameOfInsuredObject, testRisk, testPolicy.ValidFrom);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
                .WithMessage($"[Requested policy with the properties: " +
                $"'{string.Join(", ", testPolicy.NameOfInsuredObject, testPolicy.ValidFrom)}' is not found]");
        }

        [Fact]
        public void FindPolicy_InputValid_ReturnsPolicy()
        {
            // Arrange
            var testName = "BMW 330 2022";
            var testValidFrom = new DateTime(2022, 01, 01);

            // Act
            var actual = _sut.FindPolicy(testName, testValidFrom);

            // Assert
            actual.Should().BeSameAs(_registeredPolicies[0]);
        }

        [Fact]
        public void FindPolicy_InputInvalid_ThrowsException()
        {
            // Arrange
            var testName = "BMW 335 2022";
            var testValidFrom = new DateTime(2022, 01, 01);

            // Act
            Action action = () => _sut.FindPolicy(testName, testValidFrom);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
                .WithMessage($"[Requested policy with the properties: '{string.Join(", ", testName, testValidFrom)}' is not found]");
        }
    }
}