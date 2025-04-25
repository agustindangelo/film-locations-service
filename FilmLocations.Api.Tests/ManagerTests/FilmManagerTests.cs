using FilmLocations.Api.Managers.Implementations;
using FilmLocations.Api.Models;
using FilmLocations.Api.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace FilmLocations.Api.Tests.ManagerTests;

[TestFixture]
public class FilmManagerTests
{
    private Mock<IFilmRepository> _mockFilmRepository;
    private Mock<ILogger<FilmManager>> _mockLogger;
    private FilmManager _FilmManager;

    [SetUp]
    public void SetUp()
    {
        _mockFilmRepository = new Mock<IFilmRepository>();
        _mockLogger = new Mock<ILogger<FilmManager>>();
        _FilmManager = new FilmManager(_mockFilmRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetFilmDetails_ReturnsFilmLocation_WhenFilmLocationExists()
    {
        // Arrange
        var filmId = "aRandomUUID";
        var filmLocation = new FilmLocation { Id = filmId, Title = "dummy film title" };
        var expectedFilm = new FilmLocation { Id = filmId, Title = "dummy film title" };
        _mockFilmRepository.Setup(r => r.GetFilmDetails(filmId)).ReturnsAsync(filmLocation);

        // Act
        var result = await _FilmManager.GetFilmDetails(filmId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedFilm));
    }

    [Test]
    public async Task SearchFilms_ReturnsFilms_WhenFilmsExist()
    {
        // Arrange
        var searchInput = "film title";
        var films = new List<FilmLocation> { new FilmLocation { Id = "dummyUUID", Title = "dummy film title" } };
        var expectedfilms = new List<FilmLocation> { new FilmLocation { Id = "dummyUUID", Title = "dummy film title" } };
        _mockFilmRepository.Setup(r => r.Search(searchInput)).ReturnsAsync(films);

        // Act
        var result = await _FilmManager.Search(searchInput);

        // Assert
        Assert.That(result, Is.EqualTo(expectedfilms));
    }

    [Test]
    public void SearchFilms_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var searchInput = "test";
        _mockFilmRepository.Setup(r => r.Search(searchInput)).ThrowsAsync(new Exception("Repository error"));

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _FilmManager.Search(searchInput));
    }
}