namespace InsuranceService
{
    public struct Risk
    {
        public Risk(string insuredRisk, decimal yearlyPrice, DateTime effectiveDate)
        {
            Name = insuredRisk;
            YearlyPrice = yearlyPrice;
            EffectiveDate = effectiveDate;
        }

        public string Name { get; set; }

        public decimal YearlyPrice { get; set; }

        public DateTime EffectiveDate { get; set; }
    }
}