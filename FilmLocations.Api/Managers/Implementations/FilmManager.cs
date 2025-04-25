using FilmLocations.Api.Managers.Contracts;
using FilmLocations.Api.Models;
using FilmLocations.Api.Repositories.Contracts;

namespace FilmLocations.Api.Managers.Implementations;
public class FilmManager : IFilmManager
{
    private readonly IFilmRepository _filmRepository;
    private readonly ILogger<FilmManager> _logger;

    public FilmManager(IFilmRepository filmRepository, ILogger<FilmManager> logger)
    {
        _filmRepository = filmRepository;
        _logger = logger;
    }

    public async Task<FilmLocation?> GetFilmDetails(string id)
    {
        try
        {
            var film = await _filmRepository.GetFilmDetails(id);
            return film;
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
            var films = await _filmRepository.Search(inputTitle);
            return films;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while searching films with title: {inputTitle}");
            throw;
        }
    }
}