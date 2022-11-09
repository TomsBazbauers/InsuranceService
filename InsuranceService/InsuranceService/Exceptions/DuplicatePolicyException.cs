namespace InsuranceService
{
    public class DuplicatePolicyException : Exception
    {
        public DuplicatePolicyException(string info) 
            : base($"[Policy with properties: '{info}' is already registered in the system]")
        {}
    }
}