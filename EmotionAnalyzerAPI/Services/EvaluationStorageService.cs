using System.Text.Json;
using EmotionAnalyzerML.Models;

namespace EmotionAnalyzerAPI.Services
{
    public class EvaluationStorageService
    {

        private readonly string path =
            "Data/evaluation.json";


        public void Save(ModelEvaluationResult result)
        {
            var json =
                JsonSerializer.Serialize(
                    result,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });


            File.WriteAllText(
                path,
                json);
        }



        public ModelEvaluationResult? Load()
        {
            if (!File.Exists(path))
            {
                return null;
            }


            var json =
                File.ReadAllText(path);


            return JsonSerializer.Deserialize<ModelEvaluationResult>(
                json);
        }
    }
}