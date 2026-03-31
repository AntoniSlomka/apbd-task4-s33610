using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Interfaces
{
    public interface IBillingGateway
    {
        public void SaveInvoice(RenewalInvoice invoice);

        public void SendEmail(string email, string subject, string body);
    }
}
