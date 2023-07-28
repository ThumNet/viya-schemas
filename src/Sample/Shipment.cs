using Swashbuckle.AspNetCore.Annotations;

namespace Sample;

public class Shipment
{
    [RefFrom("https://raw.githubusercontent.com/ThumNet/viya-schemas/main/schemas/country-codes.schema.json")]
    public string CountryCode { get; set; }
}