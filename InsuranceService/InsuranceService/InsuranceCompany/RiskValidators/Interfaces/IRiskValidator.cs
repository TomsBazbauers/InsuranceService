namespace InsuranceService
{
    public interface IRiskValidator
    {
        bool IsValid(Risk risk);
    }
}