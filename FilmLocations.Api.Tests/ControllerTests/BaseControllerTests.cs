using FilmLocations.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FilmLocations.Api.Models;
using Moq;

namespace FilmLocations.Api.Tests.ControllerTests;

[TestFixture]
public class BaseControllerTests
{
    private class TestController : BaseController
    {
        public TestController(ILogger<BaseController> logger) : base(logger) { }

        public new IActionResult HandleSuccess<T>(T data) => base.HandleSuccess(data);
        public new IActionResult HandleError(string message) => base.HandleError(message);
        public new IActionResult HandleException(Exception ex) => base.HandleException(ex);
    }

    private Mock<ILogger<BaseController>> _mockLogger;
    private TestController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<BaseController>>();
        _controller = new TestController(_mockLogger.Object);
    }

    [Test]
    public void HandleSuccess_ReturnsOk_WhenDataIsNotNull()
    {
        // Arrange
        var data = new { Id = 1, Name = "Test" };

        // Act
        var result = _controller.HandleSuccess(data);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.EqualTo(data));
    }

    [Test]
    public void HandleSuccess_ReturnsNotFound_WhenDataIsNull()
    {
        // Act
        var result = _controller.HandleSuccess<object?>(null);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);

        var value = notFoundResult.Value as ErrorResponse;
        Assert.That(value?.Message, Is.EqualTo("Resource not found"));
    }

    [Test]
    public void HandleError_ReturnsBadRequest()
    {
        // Arrange
        var message = "Test error";

        // Act
        var result = _controller.HandleError(message);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);

        var value = badRequestResult.Value as ErrorResponse;
        Assert.That(value?.Message, Is.EqualTo("An unexpected error occurred."));
    }

    [Test]
    public void HandleException_ReturnsStatusCode500()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var result = _controller.HandleException(exception);

        // Assert
        var statusCodeResult = result as ObjectResult;
        Assert.That(statusCodeResult, Is.Not.Null);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
        var value = statusCodeResult.Value as ErrorResponse;
        Assert.That(value?.Message, Is.EqualTo("An unexpected error occurred."));
    }

    [Test]
    public void HandleSuccess_LogsWarning_WhenDataIsNull()
    {
        // Act
        _controller.HandleSuccess<object?>(null);

        // Assert logging behavior
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Resource not found")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once
        );
    }
}