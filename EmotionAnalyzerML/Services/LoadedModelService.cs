using Microsoft.ML;

namespace EmotionAnalyzerML.Services
{
// This service is responsible for loading and providing access to the trained ML model.
    public class LoadedModelService
    {
        public ITransformer Model { get; private set; }

        
        public bool IsLoaded =>
            Model != null;


        // Loads the model from the specified path using the provided ModelLoader
        public void LoadModel(
            ModelLoader loader,
            string modelPath)
        {
          
            if (IsLoaded)
            {
                return;
            }


            Model = loader.Load(modelPath);
        }


        //Return the loaded model. Throws an exception if the model has not been loaded.
        public ITransformer GetModel()
        {
            if (!IsLoaded)
            {
                throw new InvalidOperationException(
                    "Model has not been loaded.");
            }

            return Model;
        }
    }
}