using EmotionAnalyzerML.Services;
using Microsoft.ML;


namespace EmotionAnalyzerTests.Services
{
    public class EmotionPredictionServiceTests
    {
        // Tests if the service rejects empty input text
        [Fact]
        public void Predict_ShouldThrowException_WhenTextIsEmpty()
        {
            // Arrange
            var context = new MLContext();

            
            var loadedModel = new LoadedModelService();

            
            var service = new EmotionPredictionService(context,loadedModel);


            // Act + Assert

            Assert.Throws<ArgumentException>(
                () =>
                    service.Predict("")
            );
        }

        // Tests if prediction cannot be performed when the ML model has not been loaded
        [Fact]
        public void Predict_ShouldThrowException_WhenModelIsNotLoaded()
        {
            // Arrange
            var context = new MLContext();

            var loadedModel =  new LoadedModelService();

            var service = new EmotionPredictionService(context,loadedModel);

            // Act + Assert

            Assert.Throws<InvalidOperationException>(() =>service.Predict("I love this movie"));
        }

        // Verifies that the service can generate emotion predictions
        [Fact]
        public void Predict_ShouldReturnPredictions_WhenModelIsLoaded()
        {
            // Arrange

            var context = new MLContext();

            var loadedModel = new LoadedModelService();

            var loader = new ModelLoader(context);


            var modelPath = Path.Combine(AppContext.BaseDirectory,"TestData","emotion_model.zip");

            loadedModel.LoadModel(loader,modelPath);

            var service = new EmotionPredictionService(context, loadedModel);


            // Act

            var result = service.Predict("I am very happy and I love this day");


            // Assert


            Assert.NotNull(result);

            Assert.NotNull(result.Predictions);

            Assert.NotEmpty(result.Predictions);

            Assert.Equal(6,result.Predictions.Count);
        }

        // Verifies that the prediction result contains all expected emotion labels
        [Fact]
        public void Predict_ShouldReturnAllEmotionLabels_WhenModelIsLoaded()
        {
            // Arrange

            var context = new MLContext();


            var loadedModel = new LoadedModelService();

            var loader = new ModelLoader(context);

            var modelPath = Path.Combine(AppContext.BaseDirectory,"TestData","emotion_model.zip");


            loadedModel.LoadModel(loader, modelPath);


            var service = new EmotionPredictionService(context,loadedModel);

            // Act

            var result = service.Predict("I am extremely happy and excited today");


            var emotions =
                result.Predictions
                .Select(x => x.Emotion)
                .ToList();

            // Assert


            Assert.Contains("sadness",emotions);

            Assert.Contains("love", emotions);

            Assert.Contains("surprise", emotions);

            Assert.Contains("fear", emotions);

            Assert.Contains("joy", emotions);

            Assert.Contains("anger", emotions);

        }

        // Verifies that prediction confidence scores are valid
        [Fact]
        public void Predict_ShouldReturnValidConfidenceScores_InDescendingOrder()
        {
            // Arrange

            var context = new MLContext();


            var loadedModel = new LoadedModelService();

            var loader = new ModelLoader(context);

            var modelPath = Path.Combine(AppContext.BaseDirectory, "TestData", "emotion_model.zip");

            loadedModel.LoadModel(loader, modelPath);


            var service = new EmotionPredictionService(context, loadedModel);

            // Act

            var result = service.Predict("I am extremely happy and excited today");

            var predictions = result.Predictions;

            // Assert


            // Check that all confidence values are between 0 and 1

            foreach (var prediction in predictions)
            {
                Assert.InRange(prediction.Confidence,0,1);
            }


            // Check that predictions are sorted descending by confidence

            for (int i = 0; i < predictions.Count - 1; i++)
            {
                Assert.True(predictions[i].Confidence >= predictions[i + 1].Confidence);
            }
        }
    }
}