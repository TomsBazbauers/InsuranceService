namespace InsuranceService
{
    public class RiskAvailabilityValidator : IRiskListValidator
    {
        public bool IsValid(Risk risk, IList<Risk> availableRisks)
        {
            return availableRisks.Any(rsk => rsk.YearlyPrice == risk.YearlyPrice && rsk.Name == risk.Name) ? true : throw new InvalidRiskRequestException();
        }
    }
}