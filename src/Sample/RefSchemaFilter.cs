using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sample;

public class RefSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(string))
        {
            return;
        }

        var refFrom = context.MemberInfo.GetCustomAttributes<RefFromAttribute>().ToList();
        if (!refFrom.Any())
        {
            return;
        }

        schema.Reference = new OpenApiReference
        {
            ExternalResource = refFrom.First().RefValue
        };
    }
}