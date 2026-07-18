using System.Net.Http.Json;
using EmotionAnalyzerWeb.Models;

namespace EmotionAnalyzerWeb.Services
{
    public class EmotionApiService
    {
        private readonly HttpClient _httpClient;


        public EmotionApiService(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<EmotionPredictionResult?> Predict(
            string text)
        {
            var request = new
            {
                text = text
            };


            var response =
                await _httpClient.PostAsJsonAsync(
                    "/api/emotion/predict",
                    request);



            response.EnsureSuccessStatusCode();


            return await response.Content
                .ReadFromJsonAsync<EmotionPredictionResult>();
        }
    }
}