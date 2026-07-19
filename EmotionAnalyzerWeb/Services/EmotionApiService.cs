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



        public async Task<EmotionPredictionResult?> Predict(string text)
        {
            try
            {
                var response =
                    await _httpClient.PostAsJsonAsync(
                        "api/emotion/predict",
                        new
                        {
                            text = text
                        });


                response.EnsureSuccessStatusCode();


                return await response.Content
                    .ReadFromJsonAsync<EmotionPredictionResult>();
            }
            catch (HttpRequestException)
            {
                throw new Exception(
    "Unable to connect to the Emotion Analyzer service. " +
    "The API may be starting after inactivity. " +
    "Please try again in a few seconds or visit: ");
            }
        }

        public async Task<ModelEvaluationResult?> GetEvaluation()
        {
            var response =
                await _httpClient.GetAsync(
                    "api/model/evaluation");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    "Evaluation results are not available yet.");
            }


            return await response.Content
                .ReadFromJsonAsync<ModelEvaluationResult>();
        }
    }
}