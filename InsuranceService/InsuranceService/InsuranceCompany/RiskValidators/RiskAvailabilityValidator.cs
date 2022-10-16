namespace InsuranceService
{
    public class RiskAvailabilityValidator : IRiskListValidator
    {
        public bool IsValid(Risk risk, IList<Risk> availableRisks)
        {
            return availableRisks.Any(rsk => rsk.Equals(risk)) ? true : throw new InvalidRiskRequestException();
        }
    }
}