using FilmLocations.Api.Models;

namespace FilmLocations.Api.Managers.Contracts;
public interface IFilmManager
{
    Task<FilmLocation?> GetFilmDetails(string id);
    Task<IEnumerable<FilmLocation>> Search(string inputTitle);
}