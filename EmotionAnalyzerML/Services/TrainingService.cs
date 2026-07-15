using Microsoft.ML;
using EmotionAnalyzerML.Models;

namespace EmotionAnalyzerML.Services
{
    public class TrainingService
    {
        private readonly MLContext _context;


        public TrainingService(
            MLContext context)
        {
            _context = context;
        }


        public void TrainAndSave(
            List<TextData> trainingData,
            string modelPath)
        {
            Console.WriteLine(
                "Training model...");


            var model =
                EmotionModelTrainer.TrainModel(
                    _context,
                    trainingData);


            var dataView =
                _context.Data.LoadFromEnumerable(
                    trainingData);


            var directory = Path.GetDirectoryName(modelPath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }


            _context.Model.Save(
                model,
                dataView.Schema,
                modelPath);


            Console.WriteLine(
                "Model saved.");
        }
    }
}