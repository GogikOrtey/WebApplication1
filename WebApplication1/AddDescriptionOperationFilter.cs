using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication1
{
    public class AddDescriptionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Здесь описаны комментарии к методам, которые отображаются в Swagger

            if (context.ApiDescription.RelativePath.Equals("StructDiskC_Bufer"))
            {
                operation.Description = "**Этот метод возвращает кешированную структуру папок диска C**, если она считается корректной" +
                    "\n\n🟩🕑 *Ответ прийдёт за несколько секунд*. Swagger может чутка подвиснуть, из-за большого файла" +
                    "\n\nОднако, если с момента посленего сканирования прошло больше суток, или оно было выполнено на другом компьютере, либо в программе нет кешированного файла, то выполнится процедура сканирования, 🟧 которая займёт от 2 до 5 минут";
            }

            if (context.ApiDescription.RelativePath.Equals("StructDiskC"))
            {
                operation.Description = "**Этот метод сканирует всю структуру папок на диске С**" +
                    "\n\n🟧 *Выполнение может занять от 2 до 5 минут, будьте терпеливы*" +
                    "\n\nПодробнее про работу программы можно прочитать в описании на GitHub: [GitHub Repository](https://github.com/GogikOrtey/WebApplication1)";
            }
        }
    }
}
