namespace InsuranceService
{
    public class InvalidPolicyInfoException : Exception
    {
        public InvalidPolicyInfoException(string policyInfo) 
            : base($"[Invalid or missing policy info. Check: '{policyInfo}' to solve this problem]")
        {}
    }
}