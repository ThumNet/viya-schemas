using FluentValidation;

namespace Sample;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ValidationService> _logger;

    public ValidationService(IServiceProvider serviceProvider,
        ILogger<ValidationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task Validate<T>(T input)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == null)
        {
            _logger.LogDebug($"No applicable validator found for {typeof(T).Name}, model will be treated as valid by default");   
            return;
        }
        await validator.ValidateAndThrowAsync(input, CancellationToken.None);
    }
}

public interface IValidationService
{
    Task Validate<T>(T input);
}