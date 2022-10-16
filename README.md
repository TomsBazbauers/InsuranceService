# Insurance Service


### Description:

Think about it as a real insurance company
Implement rules which seems logical to you.

---

### Brief:

- You can update list of available risks at any time
- You can sell policy with initial list of risks
- You can add risks at any moment within policy period
- Premium must be calculated according to risk validity period
- There could be several policies with the same insured object name, but different effective date
---
- Use TDD approach
- Think about OOP design patterns and S.O.L.I.D. principles
- In case of error, throw different type of exceptions for each situation

### We are giving the interface of Insurance company

``` 
public interface IInsuranceCompany
{
    /// Name of Insurance company
    string Name { get; }

    /// List of the risks that can be insured. List can be updated at any time
    IList<Risk> AvailableRisks { get; set; }
    
    /// Sell the policy.
    IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks);
    
    /// Add risk to the policy of insured object.
    void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom);

    /// Gets policy with it's risks at the given point of time.
    IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate);
}

public struct Risk 
{
    /// Unique name of the risk
    public string Name { get; set; }

    /// Risk yearly price
    public decimal YearlyPrice { get; set; }
}

public interface IPolicy 
{
    /// Name of insured object
    string NameOfInsuredObject { get; }
    
    /// Date when policy becomes active
    DateTime ValidFrom { get; }
    
    /// Date when policy becomes inactive
    DateTime ValidTill { get; }

    /// Total price of the policy. Calculate by summing up all insured risks.
    /// Take into account that risk price is given for 1 full year. Policy/risk period can be shorter.
    decimal Premium { get; }

    /// Initially included risks or risks at specific moment of time.
    IList<Risk> InsuredRisks { get; }
}
```
