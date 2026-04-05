using LegacyRenewalApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Helper
{
    public class TaxCalculator : ITaxCalculator
    {
        public decimal CalculateTaxRate(Customer customer)
        {
            decimal taxRate = 0.20m;
            if (customer.Country == "Poland")
            {
                taxRate = 0.23m;
            }
            else if (customer.Country == "Germany")
            {
                taxRate = 0.19m;
            }
            else if (customer.Country == "Czech Republic")
            {
                taxRate = 0.21m;
            }
            else if (customer.Country == "Norway")
            {
                taxRate = 0.25m;
            }

            return taxRate;
        }
    }
}
