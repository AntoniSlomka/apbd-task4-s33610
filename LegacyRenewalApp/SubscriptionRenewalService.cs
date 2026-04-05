using LegacyRenewalApp.Helper;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Repositories;
using System;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IRenewalServiceValidator _validator;
        private readonly IBillingGateway _billingGateway;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly INoteGenerator _noteGenerator;
        private readonly IFeeCalculator _feeCalculator;
        private readonly ITaxCalculator _taxCalculator;
        
        public SubscriptionRenewalService() : this(new CustomerRepository(), 
            new SubscriptionPlanRepository(), 
            new RenewalServiceValidator(),
            new BillingGatewayAdapter(),
            new DiscountCalculator(),
            new NoteGenerator(),
            new FeeCalculator(),
            new TaxCalculator()) { }
        public SubscriptionRenewalService(ICustomerRepository customerRepository, 
            ISubscriptionRepository subscriptionRepository,
            IRenewalServiceValidator validator,
            IBillingGateway billingGateway,
            IDiscountCalculator discountCalculator,
            INoteGenerator noteGenerator,
            IFeeCalculator feeCalculator,
            ITaxCalculator taxCalculator)
        {
            _customerRepository = customerRepository;
            _subscriptionRepository = subscriptionRepository;
            _validator = validator;
            _billingGateway = billingGateway;
            _discountCalculator = discountCalculator;
            _noteGenerator = noteGenerator;
            _feeCalculator = feeCalculator;
            _taxCalculator = taxCalculator;
        }

        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {

            _validator.Validate(customerId, planCode, seatCount, paymentMethod);

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _subscriptionRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            decimal discountAmount = _discountCalculator.CalculateDiscount(customer, plan, baseAmount, seatCount);

            decimal subtotalAfterDiscount = baseAmount - discountAmount;
            bool subtotalRoundedUp = false;
            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                subtotalRoundedUp= true;
            }

            decimal supportFee = _feeCalculator.CalculateSupportFee(includePremiumSupport, normalizedPlanCode);

            decimal paymentFee = _feeCalculator.CalculatePaymentFee(normalizedPaymentMethod, subtotalAfterDiscount, supportFee);

            decimal taxRate = _taxCalculator.CalculateTaxRate(customer);

            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = taxBase * taxRate;
            decimal finalAmount = taxBase + taxAmount;

            bool finalAmountRoundedUp = false;
            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                finalAmountRoundedUp = true;
            }

            string notes = _noteGenerator.generateNotes(customer, plan, seatCount, subtotalRoundedUp, finalAmountRoundedUp);

            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(baseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(discountAmount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(supportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(paymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(taxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(finalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            _billingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                _billingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}
