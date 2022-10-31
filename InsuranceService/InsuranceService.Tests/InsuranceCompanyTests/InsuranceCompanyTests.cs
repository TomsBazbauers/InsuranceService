using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace InsuranceService.Tests
{
    public class InsuranceCompanyTests
    {
        private AutoMocker _mocker;
        private InsuranceCompany _sut;
        private IEnumerable<IRiskValidator> _riskValidators;
        private IEnumerable<IRiskListValidator> _riskListValidators;
        private Mock<IPolicyRegistry> _policyRegistryMock;
        private Policy _testPolicy;

        public InsuranceCompanyTests()
        {
            _mocker = new AutoMocker();
            _riskValidators = new List<IRiskValidator>() { new RiskInfoValidator() };
            _riskListValidators = new List<IRiskListValidator>() { new RiskRequestValidator(), new RiskAvailabilityValidator() };
            var offer = new List<Risk>() { new Risk("General Insurance", 360m), new Risk("Burglary", 120m) };
            _policyRegistryMock = _mocker.GetMock<IPolicyRegistry>();
            _testPolicy = new Policy
                (
                "AUDI A3 2022", DateTime.Now.Date, DateTime.Now.Date.AddMonths(24),
                new List<Risk>() { new Risk("General Insurance", 360m, DateTime.Now) }
                );

            _sut = new InsuranceCompany("119 Insurance", offer, _riskValidators, _riskListValidators, _policyRegistryMock.Object);
        }

        [Fact]
        public void GetPolicy_InputValid_PolicyReturnedAsExpected()
        {
            // Arrange
            _policyRegistryMock
                .Setup(m => m.FindPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom)).Returns(_testPolicy);

            // Act
            var actual = _sut.GetPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom);

            // Assert
            actual.NameOfInsuredObject.Should().Be("AUDI A3 2022");
            actual.InsuredRisks.Should().HaveCount(1);
            actual.ValidFrom.Should().Be(DateTime.Now.Date);
            actual.ValidTill.Should().Be(DateTime.Now.Date.AddMonths(24));
        }

        [Fact]
        public void GetPolicy_InputInvalidValidFrom_ThrowsException()
        {
            // Arrange
            _policyRegistryMock
                .Setup(m => m.FindPolicy(_testPolicy.NameOfInsuredObject, DateTime.MinValue))
                .Throws(new PolicyNotFoundException(string.Join(", ", _testPolicy.NameOfInsuredObject, DateTime.MinValue)));

            // Act
            Action action = () => _sut.GetPolicy(_testPolicy.NameOfInsuredObject, DateTime.MinValue);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
               .WithMessage($"[Requested policy with the properties: '{string.Join(", ", _testPolicy.NameOfInsuredObject, DateTime.MinValue)}' is not found]");
        }

        [Fact]
        public void GetPolicy_InputInvalidId_ThrowsException()
        {
            // Arrange
            _policyRegistryMock
                .Setup(m => m.FindPolicy("OPEL ASTRA 2022", _testPolicy.ValidFrom))
                .Throws(new PolicyNotFoundException(string.Join(", ", "OPEL ASTRA 2022", _testPolicy.ValidFrom)));

            // Act
            Action action = () => _sut.GetPolicy("OPEL ASTRA 2022", _testPolicy.ValidFrom);

            // Assert
            action.Should().Throw<PolicyNotFoundException>()
               .WithMessage($"[Requested policy with the properties: '{string.Join(", ", "OPEL ASTRA 2022", _testPolicy.ValidFrom)}' is not found]");
        }

        [Fact]
        public void SellPolicy_InputValid_PolicySold()
        {
            // Arrange
            _policyRegistryMock
                .Setup(m => m
                .RegisterPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom, 24, _testPolicy.InsuredRisks)).Returns(_testPolicy);

            // Act
            var actual = _sut.SellPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom, 24, _testPolicy.InsuredRisks);

            // Assert
            actual.NameOfInsuredObject.Should().Be(_testPolicy.NameOfInsuredObject);
            actual.ValidFrom.Should().Be(_testPolicy.ValidFrom);
            actual.ValidTill.Should().Be(_testPolicy.ValidTill);
            actual.Premium.Should().Be(_testPolicy.Premium);
        }

        [Fact]
        public void SellPolicy_InputInvalidRiskName_ThrowsException()
        {
            // Arrange
            _policyRegistryMock
                .Setup(m => m
                .RegisterPolicy("AUDI A4 2020", _testPolicy.ValidFrom, 24, new List<Risk>() { new Risk("", 120m) }))
                .Throws(new InvalidRiskInfoException());

            // Act
            Action action = () => _sut.SellPolicy("AUDI A4 2020", _testPolicy.ValidFrom, 24, new List<Risk>() { new Risk("", 120m) });

            // Assert
            action.Should().Throw<InvalidRiskInfoException>()
               .WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void SellPolicy_InputInvalidRisk_ThrowsException()
        {
            // Arrange
            _policyRegistryMock
                .Setup(m => m
                .RegisterPolicy("AUDI A4 2020", _testPolicy.ValidFrom, 24, new List<Risk>() { new Risk("Terrorism", 120m) }))
                .Throws(new InvalidRiskInfoException());

            // Act
            Action action = () => _sut.SellPolicy("AUDI A4 2020", _testPolicy.ValidFrom, 24, new List<Risk>() { new Risk("Terrorism", 120m) });

            // Assert
            action.Should().Throw<InvalidRiskInfoException>()
               .WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void SellPolicy_InputInvalidDuplicate_ThrowsException()
        {
            // Arrange
            _policyRegistryMock
                .Setup(m => m
                .RegisterPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom, 24, _testPolicy.InsuredRisks))
                .Returns(_testPolicy);

            _policyRegistryMock
               .Setup(m => m
               .RegisterPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom, 24, _testPolicy.InsuredRisks))
               .Throws(new DuplicatePolicyException(string.Join(", ", _testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom)));

            // Act
            Action action = () => _sut.SellPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom, 24, _testPolicy.InsuredRisks);

            // Assert
            action.Should().Throw<DuplicatePolicyException>()
                .WithMessage($"[Policy with properties: " +
                $"'{string.Join(", ", _testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom)}' is already registered in the system]");
        }

        [Fact]
        public void AddRisk_InvalidInputName_ThrowsException()
        {
            // Act
            Action action = () => _sut.AddRisk(_testPolicy.NameOfInsuredObject, new Risk("", 1000m), DateTime.Now);

            // Assert
            action.Should().Throw<InvalidRiskInfoException>().WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void AddRisk_InputInvalidPrice_ThrowsException()
        {
            // Act
            Action action = () => _sut.AddRisk(_testPolicy.NameOfInsuredObject, new Risk("General", 0m), DateTime.Now);

            // Assert
            action.Should().Throw<InvalidRiskInfoException>().WithMessage($"[Invalid risk request. Risk properties missing or invalid]");
        }

        [Fact]
        public void AddRisk_InputValid_RiskAdded()
        {
            // Arrange
            var testDate = new DateTime(2022, 01, 01);
            var testRisks = new List<Risk>() { _testPolicy.InsuredRisks[0], new Risk("Burglary", 120m, DateTime.Now) };

            _sut.SellPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom, 24, _testPolicy.InsuredRisks);
            _sut.AddRisk(_testPolicy.NameOfInsuredObject, testRisks[1], _testPolicy.ValidFrom);
            
            // Act
            _policyRegistryMock.Setup(m => m.FindPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom)).Returns(_testPolicy);
            var actual = _sut.GetPolicy(_testPolicy.NameOfInsuredObject, _testPolicy.ValidFrom);

            actual.NameOfInsuredObject.Should().Be(_testPolicy.NameOfInsuredObject);
        }
    }
}