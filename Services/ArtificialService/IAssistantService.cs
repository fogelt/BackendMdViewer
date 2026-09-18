namespace BackendMdViewer.Services.AssistantService;

public interface IAssistantService
{
  Task<string> BeautifyMarkdownAsync(string content);
}