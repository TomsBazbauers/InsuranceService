namespace InsuranceService
{
    public class PolicyListValidator : IPolicyListValidator
    {
        public bool IsFound(string nameOfInsuredObject, DateTime validFrom, IList<IPolicy> registeredPolicies)
        {
            return registeredPolicies
                .Any(policy => policy.ValidFrom == validFrom
                && policy.NameOfInsuredObject == nameOfInsuredObject.Trim().ToUpper())
                ? true : throw new PolicyNotFoundException(string.Join(", ", nameOfInsuredObject, validFrom));
        }

        public bool IsUnique(string nameOfInsuredObject, DateTime validFrom, IList<IPolicy> registeredPolicies)
        {
            return !registeredPolicies
                .Any(policy => policy.ValidFrom == validFrom
                && policy.NameOfInsuredObject == nameOfInsuredObject.Trim().ToUpper())
                ? true : throw new DuplicatePolicyException(string.Join(", ", nameOfInsuredObject, validFrom));
        }
    }
}
