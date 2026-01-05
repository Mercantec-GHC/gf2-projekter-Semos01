using Microsoft.Data.Sqlite;

namespace Enterprice
{
    public class TimeStampService
    {
        // Forbindelsesstreng til SQLite-databasen (filen oprettes automatisk hvis den ikke findes)
        private const string ConnectionString = "Data Source=timestamp.db";

        public TimeStampService()
        {
            // Opret tabel hvis den ikke allerede eksisterer
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText =
            """
            CREATE TABLE IF NOT EXISTS TimeStamps (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                Time TEXT NOT NULL,
                Type TEXT NOT NULL
            );
            """;

            cmd.ExecuteNonQuery();
        }       

        public void AutoStamp(string username)
        {
            // Check om bruger findes i AD
            if (!UserADservice.UserExists(username))
            {
                Console.WriteLine();
                Console.WriteLine($"-- Brugeren '{username}' findes ikke i AD.");
                return;
            }

            // Find sidste stempling
            string? last = GetLastStampType(username);

            // Hvis sidste var IN, stemples der nu ud
            if (last == "IN")
            {
                InsertRecord(username, "OUT");
                Console.WriteLine();
                Console.WriteLine("-- Du er stemplet UD.");
            }
            // Hvis der ikke er stemplet OUT eller ingen historik, stemples der nu IN
            else
            {
                InsertRecord(username, "IN");
                Console.WriteLine();
                Console.WriteLine("-- Du er stemplet IND.");
            }
        }

        // henter historikken og skriver det som tekst linjer
        public List<string> GetHistory(string username)
        {
            var list = new List<string>();

            // Returnerer tom liste hvis intet brugernavn er angivet
            if (string.IsNullOrWhiteSpace(username))
                return list;

            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();

            // SQL: henter alle stemplinger for en bruger, sorteret nyeste først
            cmd.CommandText =
            """
            SELECT Time, Type
            FROM TimeStamps
            WHERE Username = $u
            ORDER BY Time DESC
            """;

            cmd.Parameters.AddWithValue("$u", username);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string time = reader.GetString(0);
                string type = reader.GetString(1);
                list.Add($"{time} – {type}");
            }

            return list;
        }        
        
        // kontrollere om der sidst er stemplet ind eller ud
        private string? GetLastStampType(string username)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText =
            """
            SELECT Type
            FROM TimeStamps
            WHERE Username = $u
            ORDER BY Time DESC
            LIMIT 1
            """;

            cmd.Parameters.AddWithValue("$u", username);

            var result = cmd.ExecuteScalar();

            return result?.ToString();
        }


        // Indsætter en ny stempling i databasen
        private void InsertRecord(string username, string type)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText =
            """
            INSERT INTO TimeStamps (Username, Time, Type)
            VALUES ($u, $time, $type)
            """;

            cmd.Parameters.AddWithValue("$u", username);
            cmd.Parameters.AddWithValue("$time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //Hvilket format den skal gemme tiden I
            cmd.Parameters.AddWithValue("$type", type);

            cmd.ExecuteNonQuery();
        }

        private SqliteConnection GetConnection()
        {
            var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
