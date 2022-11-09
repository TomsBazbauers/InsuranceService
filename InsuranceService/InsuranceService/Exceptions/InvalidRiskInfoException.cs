namespace InsuranceService
{
    public class InvalidRiskInfoException : Exception
    {
        public InvalidRiskInfoException() 
            : base($"[Invalid risk request. Risk properties missing or invalid]")
        {}
    }
}