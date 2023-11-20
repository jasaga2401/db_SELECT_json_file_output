using System;
using System.Data.SqlClient;
using System.Xml;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        // Connection string for your SQL Server database
        string connectionString = "Data Source=Amaze\\SQLEXPRESS;Initial Catalog=things_to_do;Integrated Security=True";

        // SQL query to retrieve data
        string query = "SELECT * FROM dbo.tbl_items";

        // JSON file path to store the result
        string jsonFilePath = "output.json";

        // Create a SqlConnection and SqlCommand
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                // Open the connection
                connection.Open();

                // Execute the SQL query
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check if there are rows
                    if (reader.HasRows)
                    {
                        // Create a list to store the data
                        var dataList = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>();

                        // Read data and populate the list
                        while (reader.Read())
                        {
                            var rowData = new System.Collections.Generic.Dictionary<string, object>();

                            // Iterate through columns and add to dictionary
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                rowData[reader.GetName(i)] = reader[i];
                            }

                            dataList.Add(rowData);
                        }

                        // Serialize the data to JSON and write to a file
                        string jsonData = JsonConvert.SerializeObject(dataList, Newtonsoft.Json.Formatting.Indented);
                        System.IO.File.WriteAllText(jsonFilePath, jsonData);

                        Console.WriteLine("Data has been retrieved and stored in 'output.json'.");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }


                }

                // output data from the JSON file
                string jsonFilePathOut = "output.json";

                string jsonDataOut = System.IO.File.ReadAllText(jsonFilePathOut);

                List<Dictionary<string, object>> dataListOut = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonDataOut);

                // Display the read data
                foreach (var dataItem in dataListOut)
                {
                    foreach (var kvp in dataItem)
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                    Console.WriteLine();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

