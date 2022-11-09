namespace InsuranceService
{
    public interface IPolicyValidator
    {
        bool IsValid(string nameOfInsuredObject, DateTime validFrom, short duration = default);
    }
}