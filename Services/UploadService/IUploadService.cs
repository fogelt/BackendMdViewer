namespace BackendMdViewer.Services.UploadService;

public interface IUploadService
{
  Task<string> SaveMarkdownFileAsync(IFormFile file);
  string? GetMarkdownContent(string fileName);
}