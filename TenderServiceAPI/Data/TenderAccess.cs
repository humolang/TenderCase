using System.Data;
using System.Data.OleDb;

namespace TenderServiceAPI.Data
{
    // Класс доступа к данным (чтения из xls-файла)
    public class TenderAccess
    {
        // В LastModified хранится время последнего изменения xls-файла
        private long LastModified;
        // Список для хранения всех прочтенных тендеров
        private List<Tender> Tenders = new List<Tender>();

        // Метод для чтения из xls-файла
        public List<Tender> GetTenders(string? connectionString)
        {
            if (connectionString == null) 
                throw new ArgumentNullException(nameof(connectionString));

            DataTable data = new DataTable();

            using (OleDbConnection connection = 
                new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable schemaTable = connection.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables, 
                    null
                    ) ?? throw new NullReferenceException();

                // Получение времени последнего изменения файла
                long lastModified = ((DateTime)schemaTable.Rows[0][7])
                    .ToUniversalTime().Ticks;

                // Если файл был изменен,
                // то из него будут заново получены данные
                // и метод вернет обновленный список тендеров.
                // В противном случае ничего не произойдет,
                // метод вернет список тендеров, считанный в предыдущий раз.
                if (lastModified > LastModified)
                {
                    LastModified = lastModified;
                    Tenders.Clear();

                    string sheet = schemaTable
                        .Rows[0]["TABLE_NAME"]
                        .ToString() ?? "Sheet1$";

                    string commandString = $"select * from [{sheet}]";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(
                        commandString, 
                        connection
                        );

                    adapter.Fill(data);

                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        Tenders.Add(
                            CreateTender(data.Rows[i])
                            );
                    }
                }
            }

            return Tenders;
        }

        // Создание объекта tender с данными из прочтенной строки
        private Tender CreateTender(DataRow row)
        {
            Tender tender = new Tender();

            tender.Name = row["Название тендера"].ToString();
            tender.DateBegin = Convert.ToDateTime(row["Дата начала"]);
            tender.DateEnd = Convert.ToDateTime(row["Дата окончания"]);
            tender.Url = row["URL тендерной площадки"].ToString();

            return tender;
        }
    }
}
