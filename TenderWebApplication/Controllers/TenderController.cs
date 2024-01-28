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

        public async Task<IActionResult> Table()
        {
            string json = await GetTendersJson();
            List<Tender> tenders = JsonSerializer
            .Deserialize<List<Tender>>(json) ?? new List<Tender>();

            return View(tenders);
        }

        private async Task<string> GetTendersJson()
        {
            using HttpResponseMessage response =
                await Program.HttpClient.GetAsync("/tenders");

            string json = await response
                .Content.ReadAsStringAsync();

            return json;
        }
    }
}
