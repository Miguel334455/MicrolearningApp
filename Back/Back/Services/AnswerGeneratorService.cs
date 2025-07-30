using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI;
using System.Net.Http.Headers;
using System.Text;

public class AnswerGeneratorService
{
    public async Task<string> GenerateAnswer(string prompt)
    {
        var apiKey = "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";

        var requestData = new
        {
            model = "mistralai/mixtral-8x7b-instruct",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost"); // obligatorio
        httpClient.DefaultRequestHeaders.Add("X-Title", "Mi App .NET IA");

        var jsonRequest = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine(responseBody);
        }

        var result = JObject.Parse(responseBody);
        var respuesta = result["choices"]?[0]?["message"]?["content"]?.ToString();

        return respuesta ?? "No response from the model.";

    }        
}
