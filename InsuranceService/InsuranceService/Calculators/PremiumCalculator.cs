namespace InsuranceService
{
    public class PremiumCalculator
    {
        public int yearLengthM = 12;
        public decimal yearLengthD = 365.25m;
        private decimal _totalPayable;

        public decimal TotalPayable => _totalPayable;
        
        public PremiumCalculator(DateTime validFrom, DateTime validTill, IList<Risk> insuredRisks)
        {
            var totalMonthlyPremium = new List<decimal>();
            var policyPeriod = Math.Round(validTill.Subtract(validFrom).Days / (yearLengthD / yearLengthM));
            
            insuredRisks.ToList().ForEach(risk => totalMonthlyPremium.Add(risk.YearlyPrice / yearLengthM));

            _totalPayable = Math.Round(totalMonthlyPremium.Sum() * policyPeriod, 2);
        }
    }
}