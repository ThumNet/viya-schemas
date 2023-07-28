// ReSharper disable once CheckNamespace

using System.Reflection;
using System.Text.Json.Nodes;
using FluentValidation.Validators;
using Json.Schema;
using Sample;

// ReSharper disable once CheckNamespace
namespace FluentValidation;

public static class ValidationExtensions
{
    
    public static IRuleBuilderOptions<T, string?> ReferenceMustExist<T>(this IRuleBuilder<T, string?> ruleBuilder,
        HttpClient httpClient)
        => ruleBuilder.SetAsyncValidator(new ReferenceMustExistValidator<T>(httpClient));
}

public class ReferenceMustExistValidator<T> : AsyncPropertyValidator<T, string?>, IPropertyValidator
{
    private readonly HttpClient _httpClient;

    public override string Name => "ReferenceMustExistValidator";

    public ReferenceMustExistValidator(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public override async Task<bool> IsValidAsync(ValidationContext<T> context, string? value, CancellationToken cancellation)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        
        var refFrom = context.InstanceToValidate.GetType().GetProperty(context.PropertyName)
            .GetCustomAttributes<RefFromAttribute>().ToList();

        if (!refFrom.Any())
        {
            return false;
        }

        var schemaContent = await _httpClient.GetStringAsync(refFrom.First().RefValue, cancellation);
        var schema = JsonSchema.FromText(schemaContent);
        var results = ValidateAgainstSchema(schema, value);
        return results.IsValid;
    }
    
    public EvaluationResults ValidateAgainstSchema(JsonSchema schema, string value)
    {
        var jsonDoc = JsonNode.Parse($"\"{value}\"");
        var results = schema.Evaluate(jsonDoc, new EvaluationOptions
        {
            OutputFormat = OutputFormat.Hierarchical,
        });
        
        return results;
    }
}