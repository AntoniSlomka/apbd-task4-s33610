using LegacyRenewalApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Helper
{
    public class EmailGenerator : IEmailGenerator
    {
        public (string, string) generateEmail(Customer customer, string normalizedPlanCode, RenewalInvoice invoice)
        {
            string subject = "Subscription renewal invoice";
            string body =
                $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

            return (subject, body);
        }
    }
}
