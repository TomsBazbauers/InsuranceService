namespace InsuranceService
{
    public class PolicyInfoValidator : IPolicyValidator
    {
        public bool IsValid(string nameOfInsuredObject, DateTime validFrom, short duration)
        {
            return !string.IsNullOrEmpty(nameOfInsuredObject.Trim()) 
                && validFrom != DateTime.MinValue 
                && validFrom <= DateTime.Now
                && duration > 0
                ? true : throw new InvalidPolicyInfoException(string.Join(", ", nameOfInsuredObject, validFrom, duration));
        }
    }
}