using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.ML;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionAnalyzerTests.Services
{
    public class LoadedModelServiceTests
    {
        // Test to verify that the LoadModel method successfully loads a model
        [Fact]
        public void LoadModel_ShouldLoadModelSuccessfully()
        {
            // Arrange

            var mlContext = new MLContext();

            var trainer = new EmotionModelTrainer(mlContext);


            var trainingData = new List<TextData>
    {
        new TextData
        {
            Text = "I am happy",
            Emotion = "joy"
        },

        new TextData
        {
            Text = "I am sad",
            Emotion = "sadness"
        }
    };


            var model = trainer.TrainModel(trainingData);

            var modelPath = Path.Combine(Path.GetTempPath(),"test_model.zip");


            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            mlContext.Model.Save(model,dataView.Schema,modelPath);

            var loader = new ModelLoader(mlContext);

            var service = new LoadedModelService();

            // Act

            service.LoadModel(loader,modelPath);

            // Assert

            Assert.True(service.IsLoaded);


            Assert.NotNull(service.Model);

            File.Delete(modelPath);
        }

        // Tests whether GetModel returns the loaded ML model.

        [Fact]
        public void GetModel_ShouldReturnLoadedModel()
        {
            // Arrange

            var service = new LoadedModelService();

            var fakeModel = new Mock<ITransformer>();

            service.GetType().GetProperty("Model").SetValue(service,fakeModel.Object);

            // Act

            var result = service.GetModel();


            // Assert

            Assert.NotNull(result);


            Assert.Equal(fakeModel.Object,result);
        }

        // Tests whether GetModel throws an exception when the model is not loaded.
        [Fact]
        public void GetModel_ShouldThrowException_WhenModelIsNotLoaded()
        {
            // Arrange

            var service = new LoadedModelService();

            // Act & Assert

            var exception = Assert.Throws<InvalidOperationException>(() => service.GetModel());

            Assert.Equal("Model has not been loaded.",exception.Message);
        }
    }
}
