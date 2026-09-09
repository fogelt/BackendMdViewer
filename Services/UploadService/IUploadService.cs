namespace BackendMdViewer.Services.UploadService;

public interface IUploadService
{
  Task<string> SaveMarkdownFileAsync(IFormFile file);
  IEnumerable<string> ListMarkdownFiles();
  string? GetMarkdownContent(string fileName);
  Task UpdateMarkdownFileAsync(string fileName, IFormFile file);
  Task DeleteMarkdownFileAsync(string fileName);
}
