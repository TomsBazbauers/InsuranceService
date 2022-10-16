namespace InsuranceService
{
    public struct Risk
    {
        public Risk(string insuredRisk, decimal yearlyPrice)
        {
            Name = insuredRisk;
            YearlyPrice = yearlyPrice;
        }

        public string Name { get; set; }

        public decimal YearlyPrice { get; set; }
    }
}