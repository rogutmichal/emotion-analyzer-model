using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionAnalyzerTests.Services
{
    public class TrainingServiceTest
    {

        // Verifies that the trained model is successfully saved to the specified file
        [Fact]
        public void TrainAndSave_ShouldCreateModelFile()
        {
            // Arrange

            var context = new MLContext();

            var trainer = new EmotionModelTrainer(context);

            var service = new TrainingService(context,trainer);



            var trainingData =
                new List<TextData>
                {
            new()
            {
                Text = "I am happy",
                Emotion = "joy"
            },

            new()
            {
                Text = "I feel amazing",
                Emotion = "joy"
            },

            new()
            {
                Text = "I am sad",
                Emotion = "sadness"
            },

            new()
            {
                Text = "This is terrible",
                Emotion = "anger"
            },

            new()
            {
                Text = "I love you",
                Emotion = "love"
            },

            new()
            {
                Text = "I am scared",
                Emotion = "fear"
            },

            new()
            {
                Text = "Wow!",
                Emotion = "surprise"
            }
                };


            var tempDirectory = Path.Combine(Path.GetTempPath(),Guid.NewGuid().ToString());


            var modelPath =Path.Combine(tempDirectory,"emotion_model.zip");

            // Act

            service.TrainAndSave(trainingData,modelPath);

            // Assert

            Assert.True(File.Exists(modelPath));


            // Cleanup

            Directory.Delete(tempDirectory,true);
        }

        // Verifies that the TrainAndSave method creates the directory if it does not exist
        [Fact]
        public void TrainAndSave_ShouldCreateDirectory_WhenDirectoryDoesNotExist()
        {
            // Arrange

            var mlContext = new MLContext();


            var trainer = new EmotionModelTrainer(mlContext);


            var service = new TrainingService(mlContext,trainer);

            var trainingData = new List<TextData>
    {
        new TextData
        {
            Text = "I am very happy today",
            Emotion = "joy"
        },
        new TextData
        {
            Text = "I feel very sad",
            Emotion = "sadness"
        },
        new TextData
        {
            Text = "I love this",
            Emotion = "love"
        }
    };


            var tempDirectory = Path.Combine(Path.GetTempPath(),"EmotionModelTests",Guid.NewGuid().ToString());


            var modelPath = Path.Combine(tempDirectory,"model.zip");


            // Act

            service.TrainAndSave(trainingData,modelPath);


            // Assert

            Assert.True(Directory.Exists(tempDirectory));


            Assert.True(File.Exists(modelPath));

            // Cleanup

            Directory.Delete(tempDirectory,true);
        }

        // Verifies that the trained model can be loaded successfully after being saved
        [Fact]
        public void TrainAndSave_ShouldCreateLoadableModel()
        {
            // Arrange

            var mlContext = new MLContext();

            var trainer = new EmotionModelTrainer(mlContext);

            var trainingService = new TrainingService(mlContext,trainer);

            var trainingData = new List<TextData>
    {
        new TextData
        {
            Text = "I am extremely happy",
            Emotion = "joy"
        },

        new TextData
        {
            Text = "I feel very sad",
            Emotion = "sadness"
        },

        new TextData
        {
            Text = "I love this amazing thing",
            Emotion = "love"
        },

        new TextData
        {
            Text = "I am angry",
            Emotion = "anger"
        }
    };



            var modelPath = Path.Combine(Path.GetTempPath(),"emotion_test_model.zip");


            // Act

            trainingService.TrainAndSave(trainingData,modelPath);

            var loadedModel = mlContext.Model.Load(modelPath,out _);

            // Assert

            Assert.NotNull(loadedModel);

            Assert.True(File.Exists(modelPath));


            // Cleanup

            File.Delete(modelPath);
        }
    }
}
