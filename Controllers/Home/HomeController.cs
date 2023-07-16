using Microsoft.AspNetCore.Mvc;
using WebApp.Services;


namespace WebApp;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly IConfiguration _configuration;

    public HomeController(FileService fileService, IConfiguration configuration)
    {
        _fileService = fileService;
        _configuration = configuration;
    }

    [HttpGet]
    public string Index()
    {
        return "Hello!";
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync(
        IFormFile file)
    {
        if (file.Length > _configuration.GetValue<int>("FileMaxSizeInBytes"))
        {
            return BadRequest("Value too big");
        }

        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var fileid = await _fileService.UploadFileAsync(stream, file.FileName);
            return Ok(fileid);
        }

        return Ok();
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadAsync(int id)
    {
        var (file, filename) = await _fileService.ReadFileAsync(id);
        return File(file, "octet/stream", filename);
    }

}
