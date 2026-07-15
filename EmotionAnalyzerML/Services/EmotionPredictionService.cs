using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace EmotionAnalyzerML.Services
{
    public class EmotionPredictionService
    {
        private readonly MLContext _context;
        private readonly ITransformer _model;


        private static readonly string[] EmotionLabels =
        {
            "sadness",
            "anger",
            "love",
            "surprise",
            "fear",
            "joy"
        };


        public EmotionPredictionService(
            MLContext context,
            ITransformer model)
        {
            _context = context;
            _model = model;
        }



        public EmotionPredictionResult Predict(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Text cannot be empty");
            }



            var input = new TextData
            {
                Text = text
            };



            var dataView =
                _context.Data.LoadFromEnumerable(
                    new List<TextData>
                    {
                        input
                    });



            var transformedData =
                _model.Transform(dataView);



            var scores =
                transformedData
                .GetColumn<float[]>("Score")
                .First();



            var predictions =
                EmotionLabels
                .Select((label, index) => new EmotionScore
                {
                    Emotion = label,

                    Confidence = scores[index]
                })
                .OrderByDescending(x => x.Confidence)
                .Take(6)
                .ToList();



            return new EmotionPredictionResult
            {
                Text = text,

                Predictions = predictions
            };
        }
    }
}