using EmotionAnalyzerML.Models;
using Microsoft.Extensions.Options;

namespace EmotionAnalyzerML.Services
{
    public class ModelInitializer
    {
        private readonly LoadedModelService _loadedModel;
        private readonly ModelLoader _modelLoader;
        private readonly ModelSettings _settings;


        public ModelInitializer(
            LoadedModelService loadedModel,
            ModelLoader modelLoader,
            IOptions<ModelSettings> options)
        {
            _loadedModel = loadedModel;
            _modelLoader = modelLoader;
            _settings = options.Value;
        }


        public void Initialize()
        {
            if (_loadedModel.IsLoaded)
            {
                return;
            }


            _loadedModel.LoadModel(
                _modelLoader,
                _settings.ModelPath);
        }
    }
}