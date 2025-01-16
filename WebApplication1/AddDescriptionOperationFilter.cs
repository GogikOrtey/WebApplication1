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
                operation.Description = "Этот метод сканирует всю структуру папок на диске С. 🟧 Выполнение может занять от 2 до 5 минут\n\n" +
                    "Подробнее про работу программы можно прочитать в описании на GitHub: [GitHub Repository](https://github.com/GogikOrtey/WebApplication1)";
            }
        }
    }

    public class AddDescriptionOperationFilter2 : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath.Contains("StructDiskC"))
            {
                operation.Description = "Этот метод возвращает кешированную структуру папок диска C, если она считается корректной. 🟩🕑 Ответ приходит моментально" +
                    "\n\nОднако, если с момента посленего сканирования прошло больше суток, или оно было выполнено на другом компьютере, либо в программе нет кешированного файла, то выполнится процедура сканирования, которая займёт 🟧 от 2 до 5 минут";
            }
        }
    }

}
