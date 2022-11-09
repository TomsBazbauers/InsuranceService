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
            insuredRisks
                .ToList()
                .ForEach(risk => totalMonthlyPremium.Add(risk.YearlyPrice / yearLengthM * ExtractPeriod(risk.EffectiveDate, validTill)));

            _totalPayable = Math.Round(totalMonthlyPremium.Sum(), 2);
        }

        public decimal ExtractPeriod(DateTime start, DateTime end)
        {
            return Math.Round(end.Subtract(start).Days / (yearLengthD / yearLengthM));
        }
    }
}