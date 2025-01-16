using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StructDiskCController : ControllerBase
    {
        // GET: api/<StructDiskCController>
        [HttpGet("/StructDiskC")]
        public IEnumerable<string> Get()
        {
            string s = ScanDiscC.TestAssecc_returnString();

            // SplitStringByNewline - это процедура, которая строку с переносами типа \n
            // разбивает на массив строк, и возвращает его (для корректного вывода в JSON формате)

            //string [] response = SplitStringByNewline(GetSimpleReuest());

            //string[] response = SplitStringByNewline(s);

            string[] response = SplitStringByNewline(ScanDiscC.GetBufer_StructDiskC());

            return response;

            //yield return response;
            //yield return "Test 1";
            //return new string[] { "value1", "value2" };
        }

        [HttpGet("/StructDiskC_Bufer")]
        public IEnumerable<string> GetBufer()
        {
            string[] response = SplitStringByNewline(ScanDiscC.GetBufer_StructDiskC());
            return response;
        }

        //// GET api/<StructDiskCController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<StructDiskCController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<StructDiskCController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<StructDiskCController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        string GetSimpleReuest()
        {
            return "123\n\n456\n8";
        }

        // Разбивает строку по символам '\n', на массив строк
        static string[] SplitStringByNewline(string input)
        {
            return input.Split(new[] { '\n' }, StringSplitOptions.None);
        }
    }
}
