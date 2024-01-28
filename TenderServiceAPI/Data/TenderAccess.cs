using System.Data;
using System.Data.OleDb;

namespace TenderServiceAPI.Data
{
    public class TenderAccess
    {
        private long LastModified;
        private List<Tender> Tenders = new List<Tender>();

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

                long lastModified = ((DateTime)schemaTable.Rows[0][7])
                    .ToUniversalTime().Ticks;

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

        private Tender CreateTender(DataRow row)
        {
            Tender tender = new Tender();

            tender.Name = row["Название тендера"]
                .ToString();

            tender.DateBegin = Convert
                .ToDateTime(row["Дата начала"])
                .ToUniversalTime().Ticks;

            tender.DateEnd = Convert
                .ToDateTime(row["Дата окончания"])
                .ToUniversalTime().Ticks;

            tender.Url = row["URL тендерной площадки"]
                .ToString();

            return tender;
        }
    }
}
