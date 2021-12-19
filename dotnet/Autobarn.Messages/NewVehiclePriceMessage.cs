namespace Autobarn.Messages
{
    public class NewVehiclePriceMessage : NewVehicleMessage {
        public string CurrencyCode { get; set; }
        public int Price { get; set; }

        public static NewVehiclePriceMessage FromNewVehicleMessage(NewVehicleMessage m, int price, string currencyCode) {
            return new NewVehiclePriceMessage {
                ManufacturerName = m.ManufacturerName,
                ModelName = m.ModelName,
                Color = m.Color,
                Registration = m.Registration,
                Year = m.Year,
                ListedAt = m.ListedAt,
                Price = price,
                CurrencyCode = currencyCode
            };
        }
    }
}