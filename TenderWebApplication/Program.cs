namespace TenderWebApplication
{
    public class Program
    {
        // HTTP-клиент для соединения с TenderServiceAPI
        public static HttpClient HttpClient { get; } = 
            new HttpClient();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            // Извлечения адреса TenderServiceAPI из конфигурации веб-приложения
            string baseAddress = builder.Configuration
                .GetConnectionString("TenderServiceURL") ?? "";
            HttpClient.BaseAddress = new Uri(baseAddress);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Tender/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Tender}/{action=Index}"
                );

            app.Run();
        }
    }
}
