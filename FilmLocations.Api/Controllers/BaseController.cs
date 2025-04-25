using FilmLocations.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmLocations.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    private readonly ILogger<BaseController> _logger;

    protected BaseController(ILogger<BaseController> logger)
    {
        _logger = logger;
    }

    protected IActionResult HandleSuccess<T>(T data)
    {
        if (data == null || (data is IEnumerable<object> collection && !collection.Any()))
        {
            _logger.LogWarning("Resource not found");
            return NotFound(new ErrorResponse { Message = "Resource not found" });
        }

        return Ok(data);
    }

    protected IActionResult HandleError(string message)
    {
        _logger.LogError($"Error: {message}");
        return BadRequest(new ErrorResponse { Message = "An unexpected error occurred." });
    }

    protected IActionResult HandleException(Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred");
        return StatusCode(500, new ErrorResponse { Message = "An unexpected error occurred." });
    }
}