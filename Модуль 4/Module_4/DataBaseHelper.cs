using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_4
{
    class DataBaseHelper
    {
        private static string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=combine;
              Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool Login(string username, string password)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query =
                    "SELECT COUNT(*) FROM Users " +
                    "WHERE login=@login " +
                    "AND password=@password " +
                    "AND isActive=1";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@login", username);
                command.Parameters.AddWithValue("@password", password);

                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }

        public static bool IsBlocked(string username)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query =
                    "SELECT isActive FROM Users WHERE login=@login";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@login", username);

                object result = command.ExecuteScalar();

                if (result == null)
                    return false;

                return !(bool)result;
            }
        }

        public static void BlockUser(string username)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query =
                    "UPDATE Users SET isActive=0 WHERE login=@login";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@login", username);

                command.ExecuteNonQuery();
            }
        }
    }
}
