namespace BackendMdViewer.Services.ArtificialService;

public interface IArtificialService
{
  Task<string> BeautifyMarkdownAsync(string content);
}