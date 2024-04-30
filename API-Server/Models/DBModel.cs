using API_Server.Entities;
using Npgsql;
using System.Data;

namespace app_example_net_core.Models
{
    public class DBModel
    {
        private static string connectionString = "Server=localhost;Port=5432; Database=farms; User Id = postgres; Password = 1234";


        public static DataTable Connection(string SQL)
        {
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand();
                        command.Connection = sqlConnection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = SQL;
                        NpgsqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                        command.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("Состояние подключения: " + sqlConnection.State);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
