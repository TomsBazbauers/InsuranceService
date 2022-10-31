using FluentAssertions;
using Xunit;

namespace InsuranceService
{
    public class InsuranceCompanyTests
    {
        private IInsuranceCompany _sut;
        private readonly IEnumerable<IRiskValidator> _riskValidators;
        private readonly IEnumerable<IRiskListValidator> _riskListValidators;
        private readonly IPolicyRegistry _policyRegistry;
        private IList<IPolicy> _registeredPolicies;

        public InsuranceCompanyTests()
        {
            _riskValidators = new List<IRiskValidator>() { new RiskInfoValidator() };
            _riskListValidators = new List<IRiskListValidator>() { new RiskRequestValidator(), new RiskAvailabilityValidator() };
            _registeredPolicies = new List<IPolicy>()
             {
                 new Policy("BMW 330 2022", 
                 new DateTime(2022, 01, 01), new DateTime(2025, 01, 01), 
                 new List<Risk>() {new Risk("General", 360m, new DateTime(2022, 01, 01)) }),
                 new Policy("AUDI A3 2020", 
                 new DateTime(2023, 01, 01), new DateTime(2025, 01, 01), 
                 new List<Risk>() {new Risk("Burglary", 720m,  new DateTime(2023, 01, 01))}),
                 new Policy("VW GOLF 2015", 
                 new DateTime(2014, 03, 19), new DateTime(2028, 01, 01), 
                 new List<Risk>() {new Risk("Weather Damage", 120m, new DateTime(2014, 03, 19)) }),
                 new Policy("OPEL ZAFIRA 2021", 
                 new DateTime(2021, 04, 04), new DateTime(2030, 01, 01), 
                 new List<Risk>() {new Risk("Burglary", 720m, new DateTime(2021, 04, 04)) })
             };

            _policyRegistry = new PolicyRegistry(_registeredPolicies, 
                new List<IPolicyValidator>() { new PolicyInfoValidator() }, new PolicyListValidator());
            
            _sut = new InsuranceCompany("119 Insurance",
                new List<Risk>() {
                    new Risk("General", 360m, new DateTime(2021, 01, 01)),
                    new Risk("Burglary", 720m, new DateTime(2021, 01, 01)),
                    new Risk("Weather Damage", 120m, new DateTime(2021, 01, 01)) },
                _riskValidators, _riskListValidators, _policyRegistry);
        }

        [Fact]
        public void AddRisk_InputValid_RiskAdded()
        {
            // Arrange
            var testPolicyName = "OPEL ZAFIRA 2021";
            var testPolicyValidFrom = new DateTime(2021, 04, 04);
            var testRisk = new Risk("General", 360m, new DateTime(2021, 04, 04));

            // Act
            _sut.AddRisk(testPolicyName, testRisk, testPolicyValidFrom);

            // Assert
            _registeredPolicies[3].InsuredRisks.Count.Should().Be(2);
            _registeredPolicies[3].InsuredRisks[1].Should().Be(testRisk);
        }

        [Fact]
        public void AddRisk_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testPolicyName = "OPEL ZAFIRA 2021";
            var testPolicyValidFrom = new DateTime(2021, 04, 04);
            var testRisk = new Risk("", 360m, new DateTime(2021, 04, 04));

            // Act
            Action action = () => _sut.AddRisk(testPolicyName, testRisk, testPolicyValidFrom);

            // Assert
            action.Should().Throw<InvalidRiskInfoException>().WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
            _registeredPolicies[3].InsuredRisks.Count.Should().Be(1);
        }

        [Fact]
        public void AddRisk_InputInvalidPrice_ThrowsException()
        {
            // Arrange
            var testPolicyName = "OPEL ZAFIRA 2021";
            var testPolicyValidFrom = new DateTime(2021, 04, 04);
            var testRisk = new Risk("General", 0m, new DateTime(2021, 04, 04));

            // Act
            Action action = () => _sut.AddRisk(testPolicyName, testRisk, testPolicyValidFrom);

            // Assert
            action.Should().Throw<InvalidRiskInfoException>().WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void AddRisk_InputInvalidRiskNotOffered_ThrowsException()
        {
            // Arrange
            var testPolicyName = "OPEL ZAFIRA 2021";
            var testPolicyValidFrom = new DateTime(2021, 04, 04);
            var testRisk = new Risk("Cyber Security", 120m, new DateTime(2022, 01, 01));

            // Act
            Action action = () => _sut.AddRisk(testPolicyName, testRisk, testPolicyValidFrom);

            // Assert
            action.Should().Throw<InvalidRiskRequestException>()
               .WithMessage($"[The requested risk offer is not found]");
        }

        [Fact]
        public void GetPolicy_InputValid_ReturnsPolicy()
        {
            // Arrange
            var testPolicy = _registeredPolicies[1];

            // Act
            var actual = _sut.GetPolicy(testPolicy.NameOfInsuredObject, testPolicy.ValidFrom);

            // Assert
            actual.NameOfInsuredObject.Should().BeSameAs(testPolicy.NameOfInsuredObject);
            actual.ValidFrom.Should().BeSameDateAs(testPolicy.ValidFrom);
            actual.Premium.Should().Be(testPolicy.Premium);
        }

        [Fact]
        public void GetPolicy_InputInvalid_ThrowsException()
        {
            // Arrange
            var testPolicy = new Policy("BMW M5 1999", 
                new DateTime(2005, 10, 01), new DateTime(2025, 01, 01), 
                new List<Risk>() { new Risk("General", 360m, new DateTime(2022, 01, 01)) });

            // Act
            Action action = () => _sut.GetPolicy(testPolicy.NameOfInsuredObject, testPolicy.ValidFrom);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
                .WithMessage($"[Requested policy with the properties: " +
                $"'{string.Join(", ", testPolicy.NameOfInsuredObject, testPolicy.ValidFrom)}' is not found]");
        }

        [Fact]
        public void SellPolicy_InputValid_ReturnsPolicy()
        {
            // Arrange
            var testName = "AUDI A6 2020";
            var testValidFrom = new DateTime(2022, 01, 01);
            var testRisks = new List<Risk>() { 
                new Risk("General", 360m, new DateTime(2022, 01, 01)), 
                new Risk("Burglary", 720m, new DateTime(2022, 01, 01)) };
            short testDuration = 36;

            // Act
            var actual = _sut.SellPolicy(testName, testValidFrom, testDuration, testRisks);

            // Assert
            actual.NameOfInsuredObject.Should().BeSameAs(testName);
            actual.ValidFrom.Should().BeSameDateAs(testValidFrom);
            actual.ValidTill.Should().Be(testValidFrom.AddMonths(testDuration));
            actual.Premium.Should().Be(3240);
        }

        [Fact]
        public void SellPolicy_InputInvalidRisk_ThrowsException()
        {
            // Arrange
            var testName = "AUDI A6 2020";
            var testValidFrom = new DateTime(2022, 01, 01);
            var testRisks = new List<Risk>() {
                new Risk("Generals", 360m, new DateTime(2022, 01, 01)), 
                new Risk("Burglary", 720m, new DateTime(2022, 01, 01)) };
            short testDuration = 36;

            // Act
            Action action = () => _sut.SellPolicy(testName, testValidFrom, testDuration, testRisks);

            // Assert
            action.Should().Throw<InvalidRiskRequestException>()
                .WithMessage($"[The requested risk offer is not found]");
        }

        [Fact]
        public void SellPolicy_InputInvalidDuplicate_ThrowsException()
        {
            // Arrange
            var testPolicy = new Policy("BMW 330 2022", new DateTime(2022, 01, 01), 
                new DateTime(2025, 01, 01), new List<Risk>() { new Risk("General", 360m, new DateTime(2022, 01, 01)) });

            // Act
            Action action = () => _sut.SellPolicy(testPolicy.NameOfInsuredObject, testPolicy.ValidFrom, 12, testPolicy.InsuredRisks);

            // Assert
            action.Should().Throw<DuplicatePolicyException>()
                .WithMessage($"[Policy with properties: " +
                $"'{string.Join(", ", testPolicy.NameOfInsuredObject, testPolicy.ValidFrom)}' is already registered in the system]");
        }

        [Fact]
        public void SellPolicy_InputInvalidName_ThrowsException()
        {
            // Arrange
            var testPolicy = new Policy("", new DateTime(2022, 01, 01), 
                new DateTime(2025, 01, 01), new List<Risk>() { new Risk("General", 360m, new DateTime(2022, 01, 01)) });

            // Act
            Action action = () => _sut.SellPolicy(testPolicy.NameOfInsuredObject, testPolicy.ValidFrom, 12, testPolicy.InsuredRisks);

            // Assert
            action.Should().Throw<InvalidPolicyInfoException>()
                .WithMessage($"[Invalid or missing policy info. " +
                $"Check: '{string.Join(", ", testPolicy.NameOfInsuredObject, testPolicy.ValidFrom, 12) }' to solve this problem]");
        }
    }
}
