using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Interfaces
{
    public interface IFeeCalculator
    {
        public decimal CalculateSupportFee(bool includePremiumSupport, String normalizedPlanCode);

        public decimal CalculatePaymentFee(String normalizedPaymentMethod, decimal subtotalAfterDiscount, decimal supportFee);
    }
}
