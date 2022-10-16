using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceService
{
    public interface IPolicyRegistry
    {
        IPolicy FindPolicy(string insuredObjectName, DateTime validFrom);

        IPolicy RegisterPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks);

        void AddCoverage(string nameOfInsuredObject, Risk risk, DateTime validFrom);
    }
}