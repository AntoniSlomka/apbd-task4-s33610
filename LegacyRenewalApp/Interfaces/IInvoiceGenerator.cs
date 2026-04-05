using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Interfaces
{
    public interface IInvoiceGenerator
    {
        public RenewalInvoice generateInvoice(int customerId, String normalizedPlanCode,
            Customer customer, String normalizedPaymentMethod, int seatCount, decimal baseAmount,
            decimal discountAmount, decimal supportFee, decimal paymentFee, decimal taxAmount,
            decimal finalAmount, String notes);
    }
}
