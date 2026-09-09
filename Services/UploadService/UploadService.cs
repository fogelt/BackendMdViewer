namespace BackendMdViewer.Services.UploadService;

public class UploadService : IUploadService
{
  private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "stored_files");

  public UploadService()
  {
    if (!Directory.Exists(_storagePath))
    {
      Directory.CreateDirectory(_storagePath);
    }
  }

  public async Task<string> SaveMarkdownFileAsync(IFormFile file)
  {
    if (file == null || file.Length == 0)
      throw new ArgumentException("Invalid file.");

    if (!file.FileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
      throw new InvalidOperationException("Only .md files are allowed.");

    var fileName = Path.GetFileName(file.FileName);
    var filePath = Path.Combine(_storagePath, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    return fileName;
  }

  public string? GetMarkdownContent(string fileName)
  {
    var safeFileName = Path.GetFileName(fileName);
    var filePath = Path.Combine(_storagePath, safeFileName);

    if (!File.Exists(filePath)) return null;

    return File.ReadAllText(filePath);
  }
}