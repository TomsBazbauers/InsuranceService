namespace InsuranceService
{
    public class PolicyNotFoundException : Exception
    {
        public PolicyNotFoundException(string policyInfo) 
            : base($"[Requested policy with the properties: '{policyInfo}' is not found]")
        {}
    }
}