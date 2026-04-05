using LegacyRenewalApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Helper
{
    public class NoteGenerator : INoteGenerator
    {
        public string generateNotes(Customer customer, SubscriptionPlan plan, int seatCount, bool subtotalRoundedUp, bool finalAmountRoundedUp)
        {
            string notes = string.Empty;

            if (customer.Segment == "Silver")
            {
                notes += "silver discount; ";
            }
            else if (customer.Segment == "Gold")
            {
                notes += "gold discount; ";
            }
            else if (customer.Segment == "Platinum")
            {
                notes += "platinum discount; ";
            }
            else if (customer.Segment == "Education" && plan.IsEducationEligible)
            {
                notes += "education discount; ";
            }

            if (customer.YearsWithCompany >= 5)
            {
                notes += "long-term loyalty discount; ";
            }
            else if (customer.YearsWithCompany >= 2)
            {
                notes += "basic loyalty discount; ";
            }

            if (seatCount >= 50)
            {
                notes += "large team discount; ";
            }
            else if (seatCount >= 20)
            {
                notes += "medium team discount; ";
            }
            else if (seatCount >= 10)
            {
                notes += "small team discount; ";
            }

            if (subtotalRoundedUp)
            {
                notes += "minimum discounted subtotal applied; ";
            }

            if (finalAmountRoundedUp)
            {
                notes += "minimum invoice amount applied; ";
            }

            return notes;
        }
    }
}
