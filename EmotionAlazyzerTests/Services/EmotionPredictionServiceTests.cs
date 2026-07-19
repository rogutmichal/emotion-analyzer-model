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
            var context = new MLContext();


            var loadedModel =
                new LoadedModelService();



            var service =
                new EmotionPredictionService(
                    context,
                    loadedModel);




            Assert.Throws<ArgumentException>(
                () =>
                    service.Predict("")
            );
        }

        // Tests if prediction cannot be performed when the ML model has not been loaded
        [Fact]
        public void Predict_ShouldThrowException_WhenModelIsNotLoaded()
        {

            var context = new MLContext();


            var loadedModel =  new LoadedModelService();



            var service = new EmotionPredictionService(context,loadedModel);



            Assert.Throws<InvalidOperationException>(() =>service.Predict("I love this movie")
            );
        }
    }
}