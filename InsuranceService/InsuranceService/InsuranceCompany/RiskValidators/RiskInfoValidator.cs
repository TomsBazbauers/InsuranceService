namespace InsuranceService
{
    public class RiskInfoValidator : IRiskValidator
    {
        public bool IsValid(Risk risk, DateTime validFrom)
        {
            return !string.IsNullOrEmpty(risk.Name.Trim())
                && risk.YearlyPrice > 0
                && risk.EffectiveDate != DateTime.MinValue
                && risk.EffectiveDate <= DateTime.Now
                && risk.EffectiveDate >= validFrom
                ? true : throw new InvalidRiskInfoException();
        }
    }
}