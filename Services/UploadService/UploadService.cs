namespace BackendMdViewer.Services.UploadService;

public class UploadService : IUploadService
{
  private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "stored_files");

  public UploadService() => Directory.CreateDirectory(_storagePath);

  public async Task<string> SaveMarkdownFileAsync(IFormFile file)
  {
    if (file?.Length > 0 && file.FileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
    {
      var uniqueFileName = $"{Guid.NewGuid()}.md";
      var filePath = Path.Combine(_storagePath, uniqueFileName);

      await using var stream = new FileStream(filePath, FileMode.Create);
      await file.CopyToAsync(stream);

      return uniqueFileName;
    }

    throw new ArgumentException("Only non-empty .md files are allowed.");
  }

  public IEnumerable<string> ListMarkdownFiles() =>
    Directory.GetFiles(_storagePath, "*.md").Select(Path.GetFileName)!;

  public string? GetMarkdownContent(string fileName)
  {
    var path = GetSafePath(fileName);
    return File.Exists(path) ? File.ReadAllText(path) : null;
  }

  public async Task UpdateMarkdownFileAsync(string fileName, IFormFile file)
  {
    if (file?.Length > 0 && file.FileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
    {
      var path = GetSafePath(fileName);
      await using var stream = new FileStream(path, FileMode.Create);
      await file.CopyToAsync(stream);
      return;
    }

    throw new ArgumentException("Only non-empty .md files are allowed.");
  }

  public Task DeleteMarkdownFileAsync(string fileName)
  {
    var path = GetSafePath(fileName);
    if (File.Exists(path))
    {
      File.Delete(path);
    }
    return Task.CompletedTask;
  }

  private string GetSafePath(string fileName) =>
      Path.Combine(_storagePath, Path.GetFileName(fileName));
}