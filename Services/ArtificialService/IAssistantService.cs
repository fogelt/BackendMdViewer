namespace BackendMdViewer.Services.ArtificialService;

public interface IAssistantService
{
  Task<string> BeautifyMarkdownAsync(string content);
}