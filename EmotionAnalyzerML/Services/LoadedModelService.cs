using Microsoft.ML;

namespace EmotionAnalyzerML.Services
{
    public class LoadedModelService
    {
        public ITransformer Model { get; private set; }


        public bool IsLoaded =>
            Model != null;



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