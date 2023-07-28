using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    private readonly ILogger<ExampleController> _logger;

    public ExampleController(ILogger<ExampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetSchipment")]
    public IEnumerable<Shipment> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Shipment
            {
                CountryCode = "NL"
            })
            .ToArray();
    }
    
    [HttpPost(Name = "PostSchipment")]
    public async Task<Shipment> Post([FromBody]Shipment schipment, [FromServices] IValidationService validationService)
    {
        await validationService.Validate(schipment);
        return schipment;
    }
}