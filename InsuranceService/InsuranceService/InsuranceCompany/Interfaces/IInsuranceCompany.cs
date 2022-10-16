using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceService
{
    public interface IInsuranceCompany
    {
        string Name { get; }

        IList<Risk> AvailableRisks { get; set; }

        IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks);

        void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom);

        IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate);
    }
}