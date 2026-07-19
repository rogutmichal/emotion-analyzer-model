using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace EmotionAnalyzerML.Services
{
    public class EmotionPredictionService
    // Service for predicting emotions from text using a pre-trained ML model
    {
        private readonly MLContext _context;
        private readonly LoadedModelService _loadedModel;

        //emotion labels that the model can predict
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
            LoadedModelService loadedModel)
        {
           
            _context = context;
            _loadedModel = loadedModel;
        }


        // Predicts the emotions present in the given text
        public EmotionPredictionResult Predict(string text)
        {

            // Validate the input text
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Text cannot be empty");
            }

            // Get the pre-trained model
            var model =_loadedModel.GetModel();



            var input = new TextData
            {
                Text = text
            };


            // Create a data view from the input text
            var dataView = _context.Data.LoadFromEnumerable(
                  new List<TextData>
                    {
                        input
                    });


            // Transform the data using the model to get predictions
            var transformedData = model.Transform(dataView);


            // Extract the scores for each emotion from the transformed data
            var scores =
                transformedData
                .GetColumn<float[]>("Score")
                .First();


            // Create a list of EmotionScore objects
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