namespace InsuranceService
{
    public class PolicyRegistry : IPolicyRegistry
    {
        private readonly IList<IPolicy> _registeredPolicies;
        private readonly IEnumerable<IPolicyValidator> _policyValidators;
        private readonly IPolicyListValidator _policyListValidator;

        public PolicyRegistry(IList<IPolicy> registeredPolicies,
            IEnumerable<IPolicyValidator> policyValidator,
            IPolicyListValidator listValidator)
        {
            _registeredPolicies = registeredPolicies;
            _policyValidators = policyValidator;
            _policyListValidator = listValidator;
        }

        public IPolicy RegisterPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            IPolicy? registeredPolicy = null;

            if (_policyValidators.All(v => v.IsValid(nameOfInsuredObject, validFrom, validMonths))
                && _policyListValidator.IsUnique(nameOfInsuredObject, validFrom, _registeredPolicies))
            {

                DateTime validTill = validFrom.AddMonths(validMonths);
                registeredPolicy = new Policy(nameOfInsuredObject.Trim().ToUpper(), validFrom, validTill, selectedRisks);
                _registeredPolicies.Add(registeredPolicy);
            }

            return registeredPolicy;
        }

        public void AddCoverage(string nameOfInsuredObject, Risk risk, DateTime validFrom)
        {
            if (_policyListValidator.IsFound(nameOfInsuredObject, validFrom, _registeredPolicies))
            {
                FindPolicy(nameOfInsuredObject, validFrom).InsuredRisks.Add(risk);
            }
        }

        public IPolicy FindPolicy(string insuredObjectName, DateTime validFrom)
        {
            IPolicy? request = null;

            if (_policyListValidator.IsFound(insuredObjectName, validFrom, _registeredPolicies))
            {
                request = _registeredPolicies
                    .First(policy => policy.NameOfInsuredObject == insuredObjectName && policy.ValidFrom == validFrom);
            }

            return request;
        }
    }
}