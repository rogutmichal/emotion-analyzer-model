using EmotionAnalyzerML.Models;
using Microsoft.ML;


namespace EmotionAnalyzerML.Services
{
    // This service is responsible for training the ML.NET model
    public class TrainingService
    {
        private readonly MLContext _context;
        private readonly EmotionModelTrainer _trainer;



        
        public TrainingService(
            MLContext context,
            EmotionModelTrainer trainer)
        {
            _context = context;
            _trainer = trainer;
        }



        public void TrainAndSave(List<TextData> trainingData, string modelPath)
        {


            // Train the model using the provided training data
            var model = _trainer.TrainModel(trainingData);


            var dataView = _context.Data.LoadFromEnumerable(trainingData);


            var directory =Path.GetDirectoryName(modelPath);


            // Ensure the directory exists before saving the model
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(
                    directory);
            }

            // Save the trained model to the specified path

            _context.Model.Save(
                model,
                dataView.Schema,
                modelPath);



            Console.WriteLine(
                "Model saved.");
        }
    }
}