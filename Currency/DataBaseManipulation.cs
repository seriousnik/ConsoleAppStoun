using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Currency
{
    static class DataBaseManipulation
    {
        public static DateTime date = DateTime.Today;
        public static double GetExchangeRateByDate(string itemId, DateTime date)
        {
            if (String.IsNullOrEmpty(itemId) || date == null)
                return -1;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = $"SELECT top 1 a.VALUE/b.Nominal from dbo.currencyByDate a\n"
            + $"left join dbo.currency b\n"
            + $"ON a.item_id = b.item_id\n"
            + $"where a.date = '{date:yyyy/MM/dd}' and a.item_id = '{itemId}'";
            command.Connection = connection;
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var exR = (double)reader.GetValue(0);
                reader.Close();
                return exR;
            }
            return -1;
        }

        public static void AddDataToDbCurrency(string connectionString, List<Currency> listCurrency)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                foreach (var cur in listCurrency)
                {
                    string sqlExpression = $"INSERT INTO dbo.Currency " +
                        $"(Item_ID, Name, EngName, Nominal, ParentCode, ISO_Num_Code, ISO_Char_Code) " +
                        $"VALUES ('{cur.ID}', '{cur.Name}', '{cur.EngName}', '{cur.Nominal}', '{cur.ParentCode}', '{cur.ISO_Num_Code}', '{cur.ISO_Char_Code}')";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static void AddDataToDbCurrencyByDate(string connectionString, List<CurrencyByDate> listCurrencyByDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                foreach (var cur in listCurrencyByDate)
                {
                    string sqlExpression = $"INSERT INTO dbo.CurrencyByDate " +
                        $"(Item_ID, Value, date) " +
                        $"VALUES ('{cur.ID}', '{cur.Value}', '{date:yyyy/MM/dd}')";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
