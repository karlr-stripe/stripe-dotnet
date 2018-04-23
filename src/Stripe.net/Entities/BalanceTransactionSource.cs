using Newtonsoft.Json;
using Stripe.Infrastructure;

namespace Stripe
{
    public enum BalanceTransactionSourceType
    {
        Charge,
        Payout,
        Refund,
        Transfer,
        Unknown,
    }

    [JsonConverter(typeof(BalanceTransactionSourceConverter))]
    public class BalanceTransactionSource : StripeEntityWithId
    {
        public BalanceTransactionSourceType Type { get; set; }

        public StripeCharge Charge { get; set; }
        public StripePayout Payout { get; set; }
        public StripeRefund Refund { get; set; }
        public StripeTransfer Transfer { get; set; }
    }
}
