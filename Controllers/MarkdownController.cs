using Microsoft.AspNetCore.Mvc;
using BackendMdViewer.Services.UploadService;

namespace BackendMdViewer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarkdownController(IUploadService uploadService) : ControllerBase
{
  [HttpPost("upload")]
  public async Task<IActionResult> UploadMarkdown(IFormFile file)
  {
    try
    {
      var fileName = await uploadService.SaveMarkdownFileAsync(file);

      return CreatedAtAction(nameof(GetMarkdown), new { fileName }, new { FileName = fileName });
    }
    catch (Exception ex)
    {
      return BadRequest(new { Problem = ex.Message });
    }
  }

  [HttpGet("{fileName}")]
  public IActionResult GetMarkdown(string fileName)
  {
    var content = uploadService.GetMarkdownContent(fileName);
    if (content == null)
    {
      return NotFound(new { Problem = $"File '{fileName}' was not found." });
    }

    return Content(content, "text/markdown");
  }

  [HttpGet("allFiles")]
  public IActionResult GetAllFiles()
  {
    var files = uploadService.ListMarkdownFiles();

    if (!files.Any())
    {
      return NotFound(new { Problem = "No files were found" });
    }

    return Ok(files);
  }

  [HttpPut("{fileName}")]
  public async Task<IActionResult> UpdateMarkdown(string fileName, IFormFile file)
  {
    try
    {
      var existingContent = uploadService.GetMarkdownContent(fileName);
      if (existingContent == null)
      {
        return NotFound(new { Problem = $"Cannot update. File '{fileName}' does not exist." });
      }

      await uploadService.UpdateMarkdownFileAsync(fileName, file);

      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(new { Problem = ex.Message });
    }
  }

  [HttpDelete("{fileName}")]
  public async Task<IActionResult> DeleteMarkdown(string fileName)
  {
    try
    {
      var existingContent = uploadService.GetMarkdownContent(fileName);
      if (existingContent == null)
      {
        return NotFound(new { Problem = $"Cannot delete. File '{fileName}' does not exist." });
      }

      await uploadService.DeleteMarkdownFileAsync(fileName);

      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(new { Problem = ex.Message });
    }
  }
}
