using FluentValidation;

namespace Sample;

public class ShipmentValidator : AbstractValidator<Shipment>
{
    public ShipmentValidator(HttpClient httpClient)
    {
        RuleFor(x => x.CountryCode)
            .ReferenceMustExist(httpClient);
    }
}