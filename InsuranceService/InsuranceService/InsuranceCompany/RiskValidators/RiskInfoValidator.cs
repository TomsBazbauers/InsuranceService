namespace InsuranceService
{
    public class RiskInfoValidator : IRiskValidator
    {
        public bool IsValid(Risk risk)
        {
            return !string.IsNullOrEmpty(risk.Name.Trim()) && risk.YearlyPrice > 0
                ? true : throw new InvalidRiskInfoException();
        }
    }
}