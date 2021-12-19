using System;
using System.Threading.Tasks;
using Autobarn.PricingEngine;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer.Services {
    public class PricerService : Pricer.PricerBase {
        private readonly ILogger<PricerService> logger;
        public PricerService(ILogger<PricerService> logger) {
            this.logger = logger;
        }

        public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
            PriceReply priceReply;
            if (request.ManufacturerName == "DMC") {
                priceReply = new PriceReply() { CurrencyCode = "USD", Price = 50000 };
            } else if (request.Color.Equals("orange", StringComparison.InvariantCultureIgnoreCase)) {
                priceReply = new PriceReply { CurrencyCode = "GBP", Price = 50 };
            } else {
                priceReply = new PriceReply { CurrencyCode = "EUR", Price = 2500 };
            }
            return Task.FromResult(priceReply);
        }
    }
}
