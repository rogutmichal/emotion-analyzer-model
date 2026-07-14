using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Trainers.LightGbm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionAnalyzerML
{
    public class EmotionModelTrainer
    {
        public static ITransformer TrainModel(MLContext context, List<TextData> texts)
        {
            var classCounts = texts.GroupBy(r => r.Emotion)
                                   .ToDictionary(g => g.Key, g => g.Count());
            int maxCount = classCounts.Values.Max();

            var weightedTexts = texts.Select(r => new WeightedData
            {
                Text = r.Text,
                Emotion = r.Emotion,
                Weight = (float)maxCount / classCounts[r.Emotion]
            }).ToList();

            var trainData = context.Data.LoadFromEnumerable(weightedTexts);
            var cachedTrainData = context.Data.Cache(trainData);

            var pipeline = context.Transforms.Conversion.MapValueToKey("Label", "Emotion")
                .Append(context.Transforms.Text.NormalizeText("NormalizedText", "Text"))
                .Append(context.Transforms.Text.TokenizeIntoWords("Tokens", "NormalizedText"))
                .Append(context.Transforms.Text.RemoveDefaultStopWords("TokensClean", "Tokens"))
                .Append(context.Transforms.Conversion.MapValueToKey("TokensKeys", "TokensClean"))
                .Append(context.Transforms.Text.ProduceNgrams("UniGrams", "TokensKeys", ngramLength: 1, useAllLengths: false))
                .Append(context.Transforms.Text.ProduceNgrams("BiGrams", "TokensKeys", ngramLength: 2, useAllLengths: false))
                .Append(context.Transforms.Text.ProduceNgrams("TriGrams", "TokensKeys", ngramLength: 3, useAllLengths: false))
                .Append(context.Transforms.Concatenate("Features", "UniGrams", "BiGrams", "TriGrams"))
                .Append(context.MulticlassClassification.Trainers.LightGbm(new LightGbmMulticlassTrainer.Options
                {
                    LabelColumnName = "Label",
                    FeatureColumnName = "Features",
                    ExampleWeightColumnName = "Weight",
                    NumberOfLeaves = 104,
                    MinimumExampleCountPerLeaf = 5,
                    LearningRate = 0.006,
                    NumberOfIterations = 875,
                }))
                .Append(context.Transforms.Conversion.MapKeyToValue("PredictedEmotion", "PredictedLabel"));

            return pipeline.Fit(cachedTrainData);
        }
    }
}