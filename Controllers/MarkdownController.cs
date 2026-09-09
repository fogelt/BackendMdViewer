using Microsoft.AspNetCore.Mvc;
using BackendMdViewer.Services.UploadService;

namespace BackendMdViewer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarkdownController(IUploadService uploadService) : ControllerBase
{
  private readonly IUploadService _uploadService = uploadService;

  [HttpPost("upload")]
  public async Task<IActionResult> UploadMarkdown(IFormFile file)
  {
    try
    {
      var fileName = await _uploadService.SaveMarkdownFileAsync(file);
      return Ok(new { FileName = fileName, Message = "File uploaded successfully." });
    }
    catch (Exception ex)
    {
      return BadRequest(new { ex.Message });
    }
  }

  [HttpGet("{fileName}")]
  public IActionResult GetMarkdown(string fileName)
  {
    var content = _uploadService.GetMarkdownContent(fileName);
    if (content == null) return NotFound();
    return Content(content, "text/markdown");
  }
}