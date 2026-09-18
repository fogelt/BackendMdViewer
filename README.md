# .NET 10 Backend for [vite-md-viewer](https://github.com/fogelt/vite-md-viewer)

### Getting Started

To start the project, run the following command in your terminal:

```bash
dotnet run
```

### AI Features

This project utilizes the **Google Generative AI SDK** for seamless integration with the Gemini model (defaulting to `gemini-3.1-flash-lite`, which offers 500 free requests per day).

To enable AI features, you must obtain a Google API key and store it in your .NET User Secrets:

```bash
dotnet user-secrets set "AI:ApiKey" "YOUR_API_KEY_HERE"
```