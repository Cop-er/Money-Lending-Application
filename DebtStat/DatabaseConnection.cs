using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DebtStat
{
    class DatabaseConnection
    {
        public static string conAccess()
        {
            string relativePath = "Money Tracker.mdb";
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=xMoney.mdb";
            return connectionString;
        }

        public static string queryString()
        {
            string insertQuery = "insert into debtrecords (Name, Principal, Interest, DateStarted, TotalPayed, PaymentDates, RemainingBalance) " +
                                             "values (@Name, @Principal, @Interest, @DateStarted, @TotalPayed, @PaymentDates, @RemainingBalance)";
            return insertQuery;
        }

        public static string getAllQuery()
        {
            string query = "SELECT * FROM debtrecords";
            return query;
        }
        public static void access(String x1, String x2, String x3, String x4)
        {
            string connectionString = conAccess();
            string insertQuery = queryString();

            // Sample data to be inserted
            string name = x1;
            string principal = x2;
            string interest = x3;
            string dateStarted =x4;
            string totalPayed = "";
            string paymentDates = "";
            string remainingBalance = principal;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Principal", principal);
                        command.Parameters.AddWithValue("@Interest", interest);
                        command.Parameters.AddWithValue("@DateStarted", dateStarted);
                        command.Parameters.AddWithValue("@TotalPayed", totalPayed);
                        command.Parameters.AddWithValue("@PaymentDates", paymentDates);
                        command.Parameters.AddWithValue("@RemainingBalance", remainingBalance);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) inserted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.ReadLine();
        }

    }
}
