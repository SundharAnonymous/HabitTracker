using HabitTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HabitTracker.Infrastructure.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using HabitTracker.Domain.Interfaces;

    public class OpenAiService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHabitRepository _habitRepository;
        private readonly IAnalyticsRepository _analyticsRepository;

        public OpenAiService(IConfiguration configuration, IHabitRepository habitRepository, IAnalyticsRepository analyticsRepository)
        {
            _configuration = configuration;
            _habitRepository = habitRepository;
            _analyticsRepository = analyticsRepository;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetHabitRecommendationsAsync(int userId)
        {
            // Build a detailed prompt using the user's habit and progress data.
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("You are an AI-powered habit coach for a gamified habit tracker application.");
            promptBuilder.AppendLine("The application helps users build and maintain positive habits by tracking their daily routines and providing personalized short and sweet recommendations.");
            promptBuilder.AppendLine();

            // Retrieve user's habits from the database.
            var habits = await _habitRepository.GetUserHabitsAsync(userId);
            if (habits != null && habits.Any())
            {
                promptBuilder.AppendLine("User's habits:");
                foreach (var habit in habits)
                {
                    promptBuilder.AppendLine($"- {habit.Title}: {habit.Description} (Frequency: {habit.Frequency}).");
                }
            }
            else
            {
                promptBuilder.AppendLine("User has no recorded habits.");
            }

            promptBuilder.AppendLine();

            // Retrieve user's analytics data (progress).
            var analyticsData = await _analyticsRepository.GetUserAnalyticsAsync(userId);
            if (analyticsData != null && analyticsData.Any())
            {
                promptBuilder.AppendLine("User progress data:");
                foreach (var data in analyticsData)
                {
                    string habitName = habits
                            .Where(a => a.Id == data.HabitId)
                            .Select(a => a.Title)
                            .FirstOrDefault();
                    promptBuilder.AppendLine($"- Habit {habitName}: {data.ProgressData}, Last Updated: {data.LastUpdated:yyyy-MM-dd HH:mm}.");
                }
            }
            else
            {
                promptBuilder.AppendLine("No progress data available for the user.");
            }

            promptBuilder.AppendLine();
            promptBuilder.AppendLine("Based on the above, provide personalized, actionable habit recommendations that can help improve the user's productivity and overall well-being. Keep it short and simple and add some cool looking emojis");

            var prompt = promptBuilder.ToString();

            // Retrieve your OpenAI API key from configuration.
            var apiKey = _configuration["OpenAi:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("OpenAI API key is not configured.");

            // Use OpenAI's chat completions endpoint.
            var endpoint = "https://api.openai.com/v1/chat/completions";

            // Construct the payload using the chat completions format.
            var requestBody = new
            {
                model = "gpt-4o-mini",
                store = true,
                messages = new object[]
                {
                new { role = "system", content = "You are a helpful habit coach." },
                new { role = "user", content = prompt }
                },
                max_tokens = 200, // Adjust as necessary.
                temperature = 0.7
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            int retryCount = 0;
            int maxRetries = 5;
            int baseDelay = 2000; // Start with 2 seconds delay.

            while (retryCount < maxRetries)
            {
                var response = await _httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    using var jsonDoc = JsonDocument.Parse(responseContent);

                    // Extract the recommendation from the response.
                    var recommendation = jsonDoc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return recommendation;
                }
                else if ((int)response.StatusCode == 429) // Too Many Requests.
                {
                    int retryAfter = GetRetryAfter(response);
                    int waitTime = retryAfter > 0 ? retryAfter * 1000 : baseDelay;
                    Console.WriteLine($"Rate limit hit. Retrying in {waitTime / 1000} seconds...");
                    await Task.Delay(waitTime);
                    baseDelay *= 2; // Exponential backoff.
                    retryCount++;
                }
                else
                {
                    throw new Exception($"OpenAI API call failed: {response.StatusCode} {response.ReasonPhrase}");
                }
            }

            throw new Exception("OpenAI API failed after multiple retries.");
        }

        private int GetRetryAfter(HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues("Retry-After", out var values))
            {
                if (int.TryParse(values.FirstOrDefault(), out int retryAfterSeconds))
                {
                    return retryAfterSeconds;
                }
            }
            return 0;
        }
    }

}
