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
    public class EmotionModelTrainerTests
    {
        // Tests whether training fails when no training data is provided.

        [Fact]
        public void TrainModel_ShouldThrowException_WhenTrainingDataIsEmpty()
        {
            // Arrange

            var context = new MLContext();

            var trainer = new EmotionModelTrainer(context);

            var emptyData = new List<TextData>();

            // Act & Assert

            var exception =
               Assert.Throws<ArgumentException>(
                    () => trainer.TrainModel(emptyData));

            Assert.Equal("Training data cannot be empty.", exception.Message);
        }


        // Tests whether the trainer can create an ML.NET model from training data.

        [Fact]
        public void TrainModel_ShouldReturnTrainedModel()
        {
            // Arrange

            var context = new MLContext();

            var trainer = new EmotionModelTrainer(context);

            var trainingData =
    new List<TextData>
    {
        new TextData
        {
            Text = "I am very happy today",
            Emotion = "joy"
        },

        new TextData
        {
            Text = "This makes me smile and feel good",
            Emotion = "joy"
        },

        new TextData
        {
            Text = "I am extremely sad and lonely",
            Emotion = "sadness"
        },

        new TextData
        {
            Text = "I feel terrible and depressed",
            Emotion = "sadness"
        },

        new TextData
        {
            Text = "I love this person so much",
            Emotion = "love"
        },

        new TextData
        {
            Text = "You are amazing and I love you",
            Emotion = "love"
        },

        new TextData
        {
            Text = "I am angry and frustrated",
            Emotion = "anger"
        },

        new TextData
        {
            Text = "This situation makes me furious",
            Emotion = "anger"
        },

        new TextData
        {
            Text = "I am scared and afraid",
            Emotion = "fear"
        },

        new TextData
        {
            Text = "This is a frightening experience",
            Emotion = "fear"
        },

        new TextData
        {
            Text = "I am surprised by this result",
            Emotion = "surprise"
        },

        new TextData
        {
            Text = "I did not expect this at all",
            Emotion = "surprise"
        }
    };

            // Act

            var model = trainer.TrainModel(trainingData);

            // Assert

            Assert.NotNull(model);
        }
        
    }
}
