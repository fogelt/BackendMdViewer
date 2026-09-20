using Google.GenAI;
using Google.GenAI.Types;

namespace BackendMdViewer.Services.AssistantService;

public class AssistantService(
    IConfiguration configuration
) : IAssistantService
{
  public async Task<string> BeautifyMarkdownAsync(string content)
  {
    var apiKey = configuration["AI:ApiKey"];
    var model = configuration["AI:Model"];

    if (string.IsNullOrWhiteSpace(apiKey))
      throw new InvalidOperationException("Gemini API key is not configured.");
    if (string.IsNullOrWhiteSpace(model))
      throw new InvalidOperationException("AI model not set in appsettings.");

    var client = new Client(apiKey: apiKey);

    var config = new GenerateContentConfig
    {
      SystemInstruction = new Content
      {
        Parts = [new Part { Text = """
                    You are a Markdown formatting assistant.
                    Beautify the provided content using clean, readable Markdown syntax.
                    Preserve the original meaning and content.
                    Do not add commentary, explanations, or wrap the result in code fences.
                    Return ONLY the resulting Markdown.
                    """ }]
      }
    };

    var response = await client.Models.GenerateContentAsync(
        model: model,
        contents: content,
        config: config
    );

    var result = response.Candidates?[0]?.Content?.Parts?[0]?.Text;

    if (string.IsNullOrWhiteSpace(result))
      throw new InvalidOperationException("Gemini returned an empty response.");

    return result.Trim();
  }
}