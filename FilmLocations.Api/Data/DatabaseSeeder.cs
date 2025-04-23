using System.Globalization;
using CsvHelper;
using Microsoft.Data.Sqlite;
using Films.Api.Models;

namespace Films.Api.Data;
public static class DatabaseSeeder
{
    public static void SeedDatabase(string csvFilePath, SqliteConnection connection)
    {
        try
        {
            // Create the film_locations table if it doesn't exist
            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Films (
                    Id TEXT PRIMARY KEY,
                    Title TEXT,
                    Release_Year INTEGER,
                    Locations TEXT,
                    Fun_Facts TEXT,
                    Production_Company TEXT,
                    Distributor TEXT,
                    Director TEXT,
                    Writer TEXT,
                    Actor_1 TEXT,
                    Actor_2 TEXT,
                    Actor_3 TEXT,
                    Longitude REAL,
                    Latitude REAL,
                    Data_Loaded_At TEXT
                );";
            createTableCmd.ExecuteNonQuery();

            // Read and seed data from the CSV file
            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<Film>();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO Films (
                    Title, Release_Year, Locations, Fun_Facts,
                    Production_Company, Distributor, Director, Writer,
                    Actor_1, Actor_2, Actor_3,
                    Longitude, Latitude, Data_Loaded_At)
                VALUES (
                    $Title, $Release_Year, $Locations, $Fun_Facts,
                    $Production_Company, $Distributor, $Director, $Writer,
                    $Actor_1, $Actor_2, $Actor_3,
                    $Longitude, $Latitude, $Data_Loaded_At);";

            foreach (var record in records)
            {
                insertCmd.Parameters.Clear();
                insertCmd.Parameters.AddWithValue("$Id", Guid.NewGuid().ToString());
                insertCmd.Parameters.AddWithValue("$Title", record.Title ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Release_Year", record.Release_Year ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Locations", record.Locations ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Fun_Facts", record.Fun_Facts ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Production_Company", record.Production_Company ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Distributor", record.Distributor ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Director", record.Director ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Writer", record.Writer ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Actor_1", record.Actor_1 ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Actor_2", record.Actor_2 ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Actor_3", record.Actor_3 ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Longitude", record.Longitude ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Latitude", record.Latitude ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Data_Loaded_At", record.Data_Loaded_At ?? (object)DBNull.Value);
                insertCmd.ExecuteNonQuery();
            }
            Console.WriteLine("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }
}
