using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication1
{
    public class AddDescriptionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath.Contains("StructDiskC"))
            {
                operation.Description = "Этот эндпоинт может занять до 5 минут для обработки запроса.";
            }
        }
    }

}
