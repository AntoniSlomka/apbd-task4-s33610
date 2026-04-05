using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Interfaces
{
    public interface INoteGenerator
    {
        public String generateNotes(Customer customer, SubscriptionPlan plan, int seatCount, bool subtotalRoundedUp, bool finalAmountRoundedUp);
    }
}
