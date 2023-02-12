using System;
using Microsoft.Data.Sqlite;

namespace habit_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=habit-Tracker.db";
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
                    // case 1:
                    //     GetAllRecords();
                    //     break;
                    case "2":
                        InsertRecord();
                        break;
                    // case 3:
                    //     DeleteRecord();
                    //     break;
                    // case 4:
                    //     UpdateRecord();
                    //     break;
                    default:
                        Console.WriteLine("\nPlease enter a valid command.\n");
                        break;
                }
            }
        }

        private static void InsertRecord()
        {
            string date = GetDateInput("\n\nPlease insert the date: (Fromat: dd-mm-yy). Type 0 to return to main menu.");
            int quantity = GetNumberInput("\n\nPlease insert the number of glasses or other measure of your choice (no decimals allowed). Type 0 to return to main menu.\n\n");

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
    }
}