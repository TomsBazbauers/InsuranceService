namespace InsuranceService
{
    public class Policy : IPolicy
    {
        private readonly string _insuredObject;
        private readonly DateTime _validFrom;
        private readonly DateTime _validTill;
        private IList<Risk> _insuredRisks;

        public Policy(string insuredObject, DateTime validFrom, DateTime validTill, IList<Risk> insuredRisks)
        {
            _insuredObject = insuredObject;
            _validFrom = validFrom;
            _validTill = validTill; 
            _insuredRisks = insuredRisks;
        }

        public string NameOfInsuredObject => _insuredObject;

        public DateTime ValidFrom => _validFrom;

        public DateTime ValidTill => _validTill;

        public decimal Premium => new PremiumCalculator(_validFrom, _validTill, _insuredRisks).TotalPayable;

        public IList<Risk> InsuredRisks => _insuredRisks;
    }
}