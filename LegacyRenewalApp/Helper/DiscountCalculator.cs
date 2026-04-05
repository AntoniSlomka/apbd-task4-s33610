using LegacyRenewalApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Helper
{
    public class DiscountCalculator : IDiscountCalculator
    {
        public decimal CalculateDiscount(Customer customer, SubscriptionPlan plan, decimal baseAmount, int seatCount)
        {
            decimal discountAmount = 0m;

            if (customer.Segment == "Silver")
            {
                discountAmount += baseAmount * 0.05m;
            }
            else if (customer.Segment == "Gold")
            {
                discountAmount += baseAmount * 0.10m;
            }
            else if (customer.Segment == "Platinum")
            {
                discountAmount += baseAmount * 0.15m;
            }
            else if (customer.Segment == "Education" && plan.IsEducationEligible)
            {
                discountAmount += baseAmount * 0.20m;
            }

            if (customer.YearsWithCompany >= 5)
            {
                discountAmount += baseAmount * 0.07m;
            }
            else if (customer.YearsWithCompany >= 2)
            {
                discountAmount += baseAmount * 0.03m;
            }

            if (seatCount >= 50)
            {
                discountAmount += baseAmount * 0.12m;
            }
            else if (seatCount >= 20)
            {
                discountAmount += baseAmount * 0.08m;
            }
            else if (seatCount >= 10)
            {
                discountAmount += baseAmount * 0.04m;
            }

            return discountAmount;
        }
    }
}
