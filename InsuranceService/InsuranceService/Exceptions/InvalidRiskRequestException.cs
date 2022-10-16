namespace InsuranceService
{
    public class InvalidRiskRequestException : Exception
    {
        public InvalidRiskRequestException() 
            : base($"[The requested risk offer is not found]")
        {}
    }
}