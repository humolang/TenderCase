using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TenderWebApplication.Models;

namespace TenderWebApplication.Controllers
{
    public class TenderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // В методе http-клиент совершает запрос по заданному эндпоинту
        // и возвращает результат как json-строку.
        // Json-строка десериализуется в список из объектов tender
        // и передается в представление Table.
        public async Task<IActionResult> Table()
        {
            using HttpResponseMessage response =
                await Program.HttpClient.GetAsync("/tenders");

            string json = await response
                .Content.ReadAsStringAsync();

            List<Tender> tenders = JsonSerializer
                .Deserialize<List<Tender>>(json) ?? new List<Tender>();

            return View(tenders);
        }
    }
}
