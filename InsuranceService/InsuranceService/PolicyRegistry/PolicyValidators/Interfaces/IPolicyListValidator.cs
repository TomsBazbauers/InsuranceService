namespace InsuranceService
{
    public interface IPolicyListValidator
    {
        bool IsFound(string nameOfInsuredObject, DateTime validFrom, IList<IPolicy> registeredPolicies);

        bool IsUnique(string nameOfInsuredObject, DateTime validFrom, IList<IPolicy> registeredPolicies);
    }
}
