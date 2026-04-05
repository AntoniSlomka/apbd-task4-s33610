using LegacyRenewalApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyRenewalApp.Helper
{
    public class FeeCalculator : IFeeCalculator
    {
        public decimal CalculatePaymentFee(String normalizedPaymentMethod, decimal subtotalAfterDiscount, decimal supportFee)
        {
            decimal paymentFee = 0m;
            if (normalizedPaymentMethod == "CARD")
            {
                paymentFee = (subtotalAfterDiscount + supportFee) * 0.02m;
                //notes += "card payment fee; ";
            }
            else if (normalizedPaymentMethod == "BANK_TRANSFER")
            {
                paymentFee = (subtotalAfterDiscount + supportFee) * 0.01m;
                //notes += "bank transfer fee; ";
            }
            else if (normalizedPaymentMethod == "PAYPAL")
            {
                paymentFee = (subtotalAfterDiscount + supportFee) * 0.035m;
                //notes += "paypal fee; ";
            }
            else if (normalizedPaymentMethod == "INVOICE")
            {
                paymentFee = 0m;
                //notes += "invoice payment; ";
            }
            else
            {
                throw new ArgumentException("Unsupported payment method");
            }

            return paymentFee;
        }

        public decimal CalculateSupportFee(bool includePremiumSupport, String normalizedPlanCode)
        {
            decimal supportFee = 0m;
            if (includePremiumSupport)
            {
                if (normalizedPlanCode == "START")
                {
                    supportFee = 250m;
                }
                else if (normalizedPlanCode == "PRO")
                {
                    supportFee = 400m;
                }
                else if (normalizedPlanCode == "ENTERPRISE")
                {
                    supportFee = 700m;
                }

                //notes += "premium support included; ";
            }

            return supportFee;
        }
    }
}
