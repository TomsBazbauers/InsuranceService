namespace InsuranceService
{
    public class InsuranceCompany : IInsuranceCompany
    {
        private readonly IEnumerable<IRiskValidator> _riskValidators;
        private readonly IEnumerable<IRiskListValidator> _riskListValidators;
        private readonly IPolicyRegistry _policyRegistry;

        public InsuranceCompany(string name, IList<Risk> availableRisks,
            IEnumerable<IRiskValidator> validators, IEnumerable<IRiskListValidator> listValidators, IPolicyRegistry registry)
        {
            Name = name;
            AvailableRisks = availableRisks;
            _riskValidators = validators;
            _riskListValidators = listValidators;
            _policyRegistry = registry;
        }

        public string Name { get; }

        public IList<Risk> AvailableRisks { get; set; }

        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
        {
            if (_riskValidators.All(v => v.IsValid(risk))
                && _riskListValidators.All(v => v.IsValid(risk, AvailableRisks)))
            {
                _policyRegistry.AddCoverage(nameOfInsuredObject, risk, validFrom);
            }
        }

        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            return _policyRegistry.FindPolicy(nameOfInsuredObject, effectiveDate);
        }

        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            IPolicy? requestedPolicy = null;

            if (selectedRisks.All(risk => _riskValidators.All(v => v.IsValid(risk))
            && _riskListValidators.All(v => v.IsValid(risk, AvailableRisks))))
            {
                requestedPolicy = _policyRegistry.RegisterPolicy(nameOfInsuredObject, validFrom, validMonths, selectedRisks);
            }

            return requestedPolicy;
        }
    }
}