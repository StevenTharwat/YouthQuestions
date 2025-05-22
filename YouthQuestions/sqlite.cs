using System.Data.SQLite;
using System.Data;

namespace YouthQuestions
{
    static public class sqlite
    {
        static string connStr = "Data Source=DB.db3;version=3;datetimeformat=CurrentCulture;";

        static public void sqlStatment(string sql)
        {
            using (var connection = new SQLiteConnection(connStr))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sql;

                command.ExecuteNonQuery();
            }
        }

        static public string sqlReturnValue(string sql)
        {
            object value;
            using (var connection = new SQLiteConnection(connStr))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sql;

                value = command.ExecuteScalar();
            }
            if (value == null) return "";
            return value.ToString();
        }

        static public DataTable sqlReturnTable(string sql)
        {
            DataTable dt = new DataTable();
            using (var connection = new SQLiteConnection(connStr))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sql;

                using (var reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }
    }
}
