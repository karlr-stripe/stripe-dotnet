using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stripe.Infrastructure
{
    internal class BalanceTransactionSourceConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var incoming = JObject.FromObject(value);

            incoming.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var incoming = JObject.Load(reader);

            var source = new BalanceTransactionSource
            {
                Id = incoming.SelectToken("id").ToString()
            };

            if (incoming.SelectToken("object")?.ToString() == "charge")
            {
                source.Type = BalanceTransactionSourceType.Charge;
                source.Charge = Mapper<StripeCharge>.MapFromJson(incoming.ToString());
            }
            else if (incoming.SelectToken("object")?.ToString() == "payout")
            {
                source.Type = BalanceTransactionSourceType.Payout;
                source.Payout = Mapper<StripePayout>.MapFromJson(incoming.ToString());
            }
            else if (incoming.SelectToken("object")?.ToString() == "refund")
            {
                source.Type = BalanceTransactionSourceType.Refund;
                source.Refund = Mapper<StripeRefund>.MapFromJson(incoming.ToString());
            }
            else if (incoming.SelectToken("object")?.ToString() == "transfer")
            {
                source.Type = BalanceTransactionSourceType.Transfer;
                source.Transfer = Mapper<StripeTransfer>.MapFromJson(incoming.ToString());
            }
            else
            {
                source.Type = BalanceTransactionSourceType.Unknown;
            }

            return source;
        }
    }
}