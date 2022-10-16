namespace InsuranceService
{
    public interface IPolicy
    {
        string NameOfInsuredObject { get; }

        DateTime ValidFrom { get; }

        DateTime ValidTill { get; }

        decimal Premium { get; }

        IList<Risk> InsuredRisks { get; }
    }
}