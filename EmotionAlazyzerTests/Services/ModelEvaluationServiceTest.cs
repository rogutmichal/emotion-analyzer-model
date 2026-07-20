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
    public class ModelEvaluationServiceTest
    {
        // Tests whether the evaluation service returns evaluation results

        [Fact]
        public void Evaluate_ShouldReturnEvaluationResults()
        {
            // Arrange

            var context = new MLContext();


            var loader = new ModelLoader(context);


            var model = loader.Load("TestData/emotion_model.zip");


            var evaluationService = new ModelEvaluationService(context);

            var testData =
                new List<TextData>
                {
            new TextData
            {
                Text = "I am very happy today",
                Emotion = "joy"
            },

            new TextData
            {
                Text = "I feel very sad and lonely",
                Emotion = "sadness"
            },

            new TextData
            {
                Text = "I love you so much",
                Emotion = "love"
            },

            new TextData
            {
                Text = "I am angry about this",
                Emotion = "anger"
            },

            new TextData
            {
                Text = "I am afraid",
                Emotion = "fear"
            },

            new TextData
            {
                Text = "I am surprised",
                Emotion = "surprise"
            }
                };

            // Act

            var result =
                evaluationService.Evaluate(
                    model,
                    testData,
                    "TEST");



            // Assert

            Assert.NotNull(result);

            Assert.Equal("TEST", result.DatasetName);
        }

        // Tests whether evaluation metrics are calculated within valid ranges.

        [Fact]
        public void Evaluate_ShouldReturnValidMetrics()
        {
            // Arrange

            var context = new MLContext();

            var loader = new ModelLoader(context);

            var model = loader.Load("TestData/emotion_model.zip");

            var service = new ModelEvaluationService(context);

            var data =
                new List<TextData>
                {
            new TextData
            {
                Text = "I am happy",
                Emotion = "joy"
            },

            new TextData
            {
                Text = "I feel sad",
                Emotion = "sadness"
            }
                };



            // Act

            var result = service.Evaluate(model,data, "TEST");


            // Assert

            Assert.InRange(result.MicroAccuracy,0,1);

            Assert.InRange(result.MacroAccuracy, 0, 1);

            Assert.True(result.LogLoss >= 0);
        }


        [Fact]
        public void Evaluate_ShouldReturnAllEmotionLabels()
        {
            // Arrange

            var context = new MLContext();

            var loader = new ModelLoader(context);

            var model = loader.Load("TestData/emotion_model.zip");

            var service =
                new ModelEvaluationService(context);


            var data =
                new List<TextData>
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
            },

            new TextData
            {
                Text = "I love this",
                Emotion = "love"
            },

            new TextData
            {
                Text = "I hate this",
                Emotion = "anger"
            },

            new TextData
            {
                Text = "I am scared",
                Emotion = "fear"
            },

            new TextData
            {
                Text = "I am surprised",
                Emotion = "surprise"
            }
                };



            // Act

            var result = service.Evaluate(model, data, "TEST");


            // Assert

            Assert.Contains(
                "joy",
                result.Labels);


            Assert.Contains(
                "sadness",
                result.Labels);


            Assert.Contains(
                "love",
                result.Labels);


            Assert.Contains(
                "anger",
                result.Labels);


            Assert.Contains(
                "fear",
                result.Labels);


            Assert.Contains(
                "surprise",
                result.Labels);
        }
    }
}
