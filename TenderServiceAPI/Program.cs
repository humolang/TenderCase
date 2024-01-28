using System.Text;
using System.Text.Json;
using TenderServiceAPI.Data;

namespace TenderServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            string? connectionString = builder.Configuration
                .GetConnectionString("TenderXLS");
            TenderAccess tenderAccess = new TenderAccess();

            app.MapGet("/tenders", () => GetTenders(
                connectionString, 
                tenderAccess
                ));

            app.Run();
        }

        private static IResult GetTenders(
            string? connectionString, 
            TenderAccess tenderAccess
            )
        {
            List<Tender> tenders = tenderAccess
                .GetTenders(connectionString);

            string jsonString = JsonSerializer
                .Serialize(tenders);

            return Results.Content(
                jsonString,
                "application/json",
                Encoding.UTF8
                );
        }
    }
}
