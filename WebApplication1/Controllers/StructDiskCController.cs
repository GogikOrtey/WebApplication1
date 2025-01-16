using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StructDiskCController : ControllerBase
    {
        // Запрос на получение буферных значений
        [HttpGet("/StructDiskC_Bufer")]
        public IEnumerable<string> GetBufer()
        {
            string[] response = SplitStringByNewline(ScanDiscC.GetBufer_StructDiskC());
            return response;
        }

        // Запрос на новое сканирование диска
        [HttpGet("/StructDiskC")]
        public IEnumerable<string> Get()
        {
            //string s = ScanDiscC.TestAssecc_returnString();

            string[] response = SplitStringByNewline(ScanDiscC.GetCurrent_StructDiskC());            
            return response;
        }



        // Процедура, которая строку с переносами типа \n
        // разбивает на массив строк, и возвращает его (для корректного вывода в JSON формате)
        static string[] SplitStringByNewline(string input)
        {
            string[] outp =  input.Split(new[] { '\n' }, StringSplitOptions.None);
            Array.Resize(ref outp, outp.Length - 1); // Удаляем последний элемент массива, т.к. это пустая строка
            return outp;
        }
    }
}
