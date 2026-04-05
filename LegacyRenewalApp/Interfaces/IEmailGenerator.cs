using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Interfaces
{
    public interface IEmailGenerator
    {
        public (String, String) generateEmail(Customer customer, String normalizedPlanCode, RenewalInvoice invoice);
    }
}
