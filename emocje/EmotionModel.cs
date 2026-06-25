using emocje.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emocje
{
    public class EmotionModel
    {
        // Etykiety emocji w kolejności odpowiadającej indeksom w wektorze "Score"

        private static readonly string[] EmotionLabels = { "sadness", "anger", "love", "surprise", "fear", "joy" };

        public static Dictionary<string, float> PredictEmotion(MLContext context, ITransformer model, TextData review)
        {
            var reviewData = new List<TextData> { review };
            var testDataView = context.Data.LoadFromEnumerable(reviewData);
            // Przepuszczenie danych przez wytrenowany model

            var transformedData = model.Transform(testDataView);
            var scores = transformedData.GetColumn<float[]>("Score").FirstOrDefault();

            // Mapowanie wyników na etykiety emocji

            var emotionProbabilities = EmotionLabels
                .Select((label, index) => new { label, score = scores[index] })
                .ToDictionary(x => x.label, x => x.score);



            // Sortowanie od największego prawdopodobieństwa
            return emotionProbabilities
                .OrderByDescending(kvp => kvp.Value)
                .Take(6)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}