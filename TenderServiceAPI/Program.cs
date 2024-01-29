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

            // Извлечение строки подключения к xls-файлу
            string? connectionString = builder.Configuration
                .GetConnectionString("TenderXLS");
            TenderAccess tenderAccess = new TenderAccess();

            app.MapGet("/tenders", () => GetTenders(
                connectionString, 
                tenderAccess
                ));

            app.Run();
        }

        // Чтение xls-файла,
        // сериализация полученных объектов
        // и отправка списка тендеров json-файлом
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
