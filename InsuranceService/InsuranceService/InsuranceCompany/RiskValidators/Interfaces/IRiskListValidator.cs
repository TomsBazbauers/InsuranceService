namespace InsuranceService
{
    public interface IRiskListValidator
    {
        bool IsValid(Risk risk, IList<Risk> availableRisks);
    }
}