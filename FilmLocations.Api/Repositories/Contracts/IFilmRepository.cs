using FilmLocations.Api.Models;
namespace FilmLocations.Api.Repositories.Contracts;
public interface IFilmRepository
{
    Task<FilmLocation?> GetFilmDetails(string id);
    Task<IEnumerable<FilmLocation>> Search(string inputTitle);

}