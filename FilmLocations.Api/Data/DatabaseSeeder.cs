using System.Globalization;
using CsvHelper;
using Microsoft.Data.Sqlite;
using FilmLocations.Api.Models;
using CsvHelper.Configuration;

namespace FilmLocations.Api.Data;
public static class DatabaseSeeder
{
    public static void SeedDatabase(string csvFilePath, SqliteConnection connection)
    {
        try
        {
            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Films (
                    Id TEXT PRIMARY KEY,
                    Title TEXT,
                    ReleaseYear INTEGER,
                    Locations TEXT,
                    FunFacts TEXT,
                    ProductionCompany TEXT,
                    Distributor TEXT,
                    Director TEXT,
                    Writer TEXT,
                    Actor1 TEXT,
                    Actor2 TEXT,
                    Actor3 TEXT,
                    Longitude REAL,
                    Latitude REAL,
                    DataLoadedAt TEXT
                );";
            createTableCmd.ExecuteNonQuery();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<FilmLocation>()
                .Where(f => !string.IsNullOrWhiteSpace(f.Locations))
                .ToList();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO Films (
                    Id, Title, ReleaseYear, Locations, FunFacts,
                    ProductionCompany, Distributor, Director, Writer,
                    Actor1, Actor2, Actor3,
                    Longitude, Latitude, DataLoadedAt)
                VALUES (
                    $Id, $Title, $ReleaseYear, $Locations, $FunFacts,
                    $ProductionCompany, $Distributor, $Director, $Writer,
                    $Actor1, $Actor2, $Actor3,
                    $Longitude, $Latitude, $DataLoadedAt);";

            foreach (var record in records)
            {
                insertCmd.Parameters.Clear();
                insertCmd.Parameters.AddWithValue("$Id", Guid.NewGuid().ToString());
                insertCmd.Parameters.AddWithValue("$Title", record.Title ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$ReleaseYear", record.ReleaseYear ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Locations", record.Locations ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$FunFacts", record.FunFacts ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$ProductionCompany", record.ProductionCompany ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Distributor", record.Distributor ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Director", record.Director ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Writer", record.Writer ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Actor1", record.Actor1 ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Actor2", record.Actor2 ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Actor3", record.Actor3 ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Longitude", record.Longitude ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$Latitude", record.Latitude ?? (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("$DataLoadedAt", record.DataLoadedAt ?? (object)DBNull.Value);
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
