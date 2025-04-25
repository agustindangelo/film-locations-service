using FilmLocations.Api.Managers.Contracts;
using FilmLocations.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FilmLocations.Api.Controllers;
[Route("api/films")]
[ApiController]
public class FilmController : BaseController
{
    private readonly IFilmManager _filmManager;
    private readonly IMemoryCache _cache;
    private MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

    public FilmController(IFilmManager filmManager, IMemoryCache cache, ILogger<BaseController> logger) : base(logger)
    {
        _filmManager = filmManager;
        _cache = cache;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFilmDetails(string id)
    {
        try
        {
            var cacheKey = $"Film_{id}";
            if (!_cache.TryGetValue(cacheKey, out FilmLocation? film))
            {
                film = await _filmManager.GetFilmDetails(id);
                if (film != null)
                {
                    _cache.Set(cacheKey, film, cacheEntryOptions);
                }
            }
            return HandleSuccess(film);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFilmByTitle([FromQuery] string title)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > 50)
            {
                throw new ArgumentException("Invalid user input.");
            }
            var cacheKey = $"Search_{title}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<FilmLocation>? films))
            {
                films = await _filmManager.Search(title);
                if (films != null)
                {
                    _cache.Set(cacheKey, films, cacheEntryOptions);
                }
            }
            return HandleSuccess(films);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}