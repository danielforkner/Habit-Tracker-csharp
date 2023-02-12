using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace habit_tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                    )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput();
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nPlease choose from the following:");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("---------------------------\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        InsertRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    // case "4":
                    //     UpdateRecord();
                    //     break;
                    default:
                        Console.WriteLine("\nPlease enter a valid command.\n");
                        break;
                }
            }
        }


        static private void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();
            string id = GetIdInput("\n\nPlease insert the id of the record you would like to delete. Type 0 to return to main menu.");
            if (id == "0") GetUserInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"DELETE FROM drinking_water WHERE id=$id";
                tableCmd.Parameters.AddWithValue("$id", id);

                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {id} does not exist. Enter to continue. 0 for Main Menu. \n\n");
                    if (Console.ReadLine() == "0") GetUserInput();
                    DeleteRecord();
                }

                Console.WriteLine($"\n\nRecord with Id {id} was successfully deleted. Enter to continue. \n\n");
                GetUserInput();
            }
        }

        internal static string GetIdInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        static private void InsertRecord()
        {
            string date = GetDateInput("\n\nPlease insert the date: (Format: mm-dd-yyyy). Type 0 to return to main menu.");
            int quantity = GetNumberInput("\n\nPlease insert the number of glasses or other measure of your choice (no decimals allowed). Type 0 to return to main menu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"INSERT INTO drinking_water (Date, Quantity) VALUES ($date, $quantity)";
                tableCmd.Parameters.AddWithValue("$date", date);
                tableCmd.Parameters.AddWithValue("$quantity", quantity);
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        internal static string GetDateInput(string message)
        {
            Console.WriteLine(message);

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"SELECT * FROM drinking_water";
                SqliteDataReader reader = tableCmd.ExecuteReader();
                List<DrinkingWater> tableData = new List<DrinkingWater>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "mm-dd-yyyy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("---------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("mm-dd-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("---------------------------\n");
            }
        }
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}