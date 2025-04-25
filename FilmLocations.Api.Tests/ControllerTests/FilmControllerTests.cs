using FilmLocations.Api.Controllers;
using FilmLocations.Api.Managers.Contracts;
using FilmLocations.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace FilmLocations.Api.Tests.ControllerTests;
[TestFixture]
public class FilmControllerTests
{
    private Mock<IFilmManager> _mockFilmManager;
    private MemoryCache _cache;
    private Mock<ILogger<BaseController>> _mockLogger;
    private FilmController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockFilmManager = new Mock<IFilmManager>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _mockLogger = new Mock<ILogger<BaseController>>();
        _controller = new FilmController(_mockFilmManager.Object, _cache, _mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _cache.Dispose();
    }

    [Test]
    public async Task GetFilmDetails_CallsFilmManager_And_CachesItem_WhenItemNotInCache()
    {
        // Arrange
        var filmId = "aRandomUUID";
        var expectedItem = new FilmLocation { Id = filmId };
        _mockFilmManager.Setup(m => m.GetFilmDetails(filmId)).ReturnsAsync(expectedItem);

        // Act
        var result = await _controller.GetFilmDetails(filmId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedItem = okResult.Value as FilmLocation;
        Assert.That(returnedItem, Is.Not.Null);
        Assert.That(returnedItem.Id, Is.EqualTo(filmId));

        Assert.That(_cache.TryGetValue($"Film_{filmId}", out FilmLocation? cachedItem), Is.True);
        Assert.That(cachedItem, Is.EqualTo(expectedItem));

        _mockFilmManager.Verify(m => m.GetFilmDetails(filmId), Times.Once);
    }

    [Test]
    public async Task GetFilmDetails_ReturnsCachedItem_WhenItemExistsInCache()
    {
        // Arrange
        var filmId = "aRandomUUID";
        var expectedItem = new FilmLocation { Id = filmId };
        _cache.Set($"Film_{filmId}", expectedItem);

        // Act
        var result = await _controller.GetFilmDetails(filmId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedItem = okResult.Value as FilmLocation;
        Assert.That(returnedItem, Is.EqualTo(expectedItem));

        _mockFilmManager.Verify(m => m.GetFilmDetails(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task GetFilmDetails_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var filmId = "aRandomUUID";
        FilmLocation? expectedItem = null;
        _mockFilmManager.Setup(m => m.GetFilmDetails(filmId)).ReturnsAsync(expectedItem);

        // Act
        var result = await _controller.GetFilmDetails(filmId);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        _mockFilmManager.Verify(m => m.GetFilmDetails(filmId), Times.Once);
    }

    [Test]
    public async Task SearchFilmByTitle_ReturnsFilms_WhenFilmsExist()
    {
        // Arrange
        var title = "test";
        var expectedFilms = new List<FilmLocation> { new FilmLocation { Id = "dummyUUID", Title = title } };
        _mockFilmManager.Setup(m => m.Search(title)).ReturnsAsync(expectedFilms);

        // Act
        var result = await _controller.SearchFilmByTitle(title);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedFilms = okResult.Value as List<FilmLocation>;
        Assert.That(returnedFilms, Is.Not.Null);
        Assert.That(returnedFilms.Count, Is.EqualTo(1));
        Assert.That(returnedFilms[0].Title, Is.EqualTo(title));
    }
}