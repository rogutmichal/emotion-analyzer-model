using EmotionAnalyzerML.Models;
using EmotionAnalyzerML;
using Microsoft.ML;

namespace emocje.Services
{
    public class EmotionPredictionService
    {
        private readonly MLContext _context;
        private readonly ITransformer _model;


        public EmotionPredictionService(
            MLContext context,
            ITransformer model)
        {
            _context = context;
            _model = model;
        }


        public Dictionary<string, float> Predict(string text)
        {
            var input = new TextData
            {
                Text = text
            };


            return EmotionModel.PredictEmotion(
                _context,
                _model,
                input);
        }
    }
}