using Microsoft.ML;


namespace EmotionAnalyzerML.Services
// This class is responsible for loading the ML.NET model
{
    public class ModelLoader
    {
        private readonly MLContext _context;


        public ModelLoader(MLContext context)
        {
            _context = context;
        }


        // Loads the ML.NET model from the specified path and returns it as an ITransformer.
        public ITransformer Load(string modelPath)
        {
            // Validate the model path
            if (string.IsNullOrWhiteSpace(modelPath))
            {
                throw new ArgumentException(
                    "Model path cannot be empty.");
            }


            // Check if the model file exists
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException(
                    "Model file was not found.",
                    modelPath);
            }
           

            return _context.Model.Load(
                modelPath,
                out _);
        }
    }
}