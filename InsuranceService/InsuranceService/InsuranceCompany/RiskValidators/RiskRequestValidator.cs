namespace InsuranceService
{
    public class RiskRequestValidator : IRiskListValidator
    {
        public bool IsValid(Risk risk, IList<Risk> availableRisks)
        {
            return availableRisks.Any(rsk => rsk.Name == risk.Name && rsk.YearlyPrice == risk.YearlyPrice)
                ? true : throw new InvalidRiskRequestException();
        }
    }
}