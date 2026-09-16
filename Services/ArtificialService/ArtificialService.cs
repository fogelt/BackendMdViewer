using System.Net.Http.Json;
using System.Text.Json;

namespace BackendMdViewer.Services.ArtificialService;

public class ArtificialService(
  HttpClient httpClient,
  IConfiguration configuration
) : IArtificialService
{
  public async Task<string> BeautifyMarkdownAsync(string content)
  {
    var apiKey = configuration["AI:ApiKey"];

    if (string.IsNullOrWhiteSpace(apiKey))
      throw new InvalidOperationException("Gemini API key is not configured.");

    var request = new
    {
      systemInstruction = new
      {
        parts = new[]
        {
          new
          {
            text = """
              You are a Markdown formatting assistant.

              Beautify the provided content using clean, readable Markdown syntax.

              Preserve the original meaning and content.
              Improve structure where appropriate using headings, paragraphs, lists,
              blockquotes, emphasis, links, and code blocks.

              Do not add commentary.
              Do not explain your changes.
              Do not wrap the result in a Markdown code fence.

              Return ONLY the resulting Markdown.
              """
          }
        }
      },
      contents = new[]
      {
        new
        {
          parts = new[]
          {
            new
            {
              text = content
            }
          }
        }
      }
    };

    var response = await httpClient.PostAsJsonAsync(
      $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}",
      request
    );

    response.EnsureSuccessStatusCode();

    using var json = await response.Content.ReadFromJsonAsync<JsonDocument>();

    var result = json?
      .RootElement
      .GetProperty("candidates")[0]
      .GetProperty("content")
      .GetProperty("parts")[0]
      .GetProperty("text")
      .GetString();

    if (string.IsNullOrWhiteSpace(result))
      throw new InvalidOperationException("Gemini returned an empty response.");

    return result.Trim();
  }
}