using System.Data;
using Dapper;
using FilmLocations.Api.Models;
using FilmLocations.Api.Repositories.Contracts;

namespace FilmLocations.Api.Repositories.Implementations;
public class FilmRepository : IFilmRepository
{
    private readonly IDbConnection _db;
    private readonly ILogger<FilmRepository> _logger;

    public FilmRepository(IDbConnection db, ILogger<FilmRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<FilmLocation?> GetFilmDetails(string id)
    {
        try
        {
            var sql = @"
            SELECT 
                Id,
                Title,
                ReleaseYear,
                Locations,
                FunFacts,
                ProductionCompany,
                Distributor,
                Director,
                Writer,
                Actor1,
                Actor2,
                Actor3,
                Longitude,
                Latitude,
                DataLoadedAt
            FROM Films 
            WHERE Id = @Id
            ORDER BY DataLoadedAt DESC";
            return await _db.QueryFirstOrDefaultAsync<FilmLocation>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting film  by id: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<FilmLocation>> Search(string inputTitle)
    {
        try
        {
            var sql = @"
            SELECT
                Id,
                Title,
                ReleaseYear,
                Locations,
                FunFacts,
                ProductionCompany,
                Distributor,
                Director,
                Writer,
                Actor1,
                Actor2,
                Actor3,
                Longitude,
                Latitude,
                DataLoadedAt
            FROM Films
            WHERE Title LIKE $SearchValue
            ORDER BY DataLoadedAt DESC";

            return await _db.QueryAsync<FilmLocation>(sql, new { SearchValue = $"%{inputTitle}%" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while searching films with input: {inputTitle}");
            throw;
        }
    }
}