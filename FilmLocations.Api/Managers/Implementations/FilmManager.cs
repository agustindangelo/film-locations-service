using AutoMapper;
using FilmLocations.Api.Managers.Contracts;
using FilmLocations.Api.Models;
using FilmLocations.Api.Repositories.Contracts;

namespace FilmLocations.Api.Managers.Implementations;
public class FilmManager : IFilmManager
{
    private readonly IFilmRepository _filmRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmManager> _logger;

    public FilmManager(IFilmRepository filmRepository, IMapper mapper, ILogger<FilmManager> logger)
    {
        _filmRepository = filmRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<FilmLocation?> GetFilmDetails(string id)
    {
        try
        {
            var film = await _filmRepository.GetFilmDetails(id);
            return _mapper.Map<FilmLocation>(film);
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
            return _mapper.Map<IEnumerable<FilmLocation>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while searching films with title: {inputTitle}");
            throw;
        }
    }
}