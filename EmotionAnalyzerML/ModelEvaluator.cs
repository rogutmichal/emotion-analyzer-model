using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionAnalyzerML
{
    public class ModelEvaluator
    {
        public static void TestModel(
            MLContext context,
            ITransformer model,
            List<TextData> reviews,
            string datasetName)
        {
            var testData = context.Data.LoadFromEnumerable(reviews);

            var labelMapping = context.Transforms.Conversion
                .MapValueToKey("Label", "Emotion");

            var testDataWithLabel = labelMapping
                .Fit(testData)
                .Transform(testData);


            var predictions = model.Transform(testDataWithLabel);

            var metrics = context.MulticlassClassification.Evaluate(
                predictions,
                labelColumnName: "Label"
            );


            Console.WriteLine($"\n=== MODEL METRICS – {datasetName.ToUpper()} ===");

            Console.WriteLine($"Accuracy Micro: {metrics.MicroAccuracy:P2}");
            Console.WriteLine($"Accuracy Macro: {metrics.MacroAccuracy:P2}");
            Console.WriteLine($"LogLoss: {metrics.LogLoss:F4}");

            Console.WriteLine("======================");


            // Pobranie nazw klas
            var labelBuffer = new VBuffer<ReadOnlyMemory<char>>();

            predictions.Schema["PredictedLabel"]
                .GetKeyValues(ref labelBuffer);

            var labels = labelBuffer.DenseValues()
                .Select(x => x.ToString())
                .ToList();


            var confusionMatrix = metrics.ConfusionMatrix;


            // Confusion Matrix
            Console.WriteLine($"\nConfusion Matrix ({datasetName}):");

            int width = 12;

            Console.Write($"{"Actual \\ Pred",-15}");

            foreach (var label in labels)
            {
                Console.Write(string.Format("{0," + width + "}", label));
            }

            Console.WriteLine();


            for (int i = 0; i < confusionMatrix.NumberOfClasses; i++)
            {
                Console.Write($"{labels[i],-15}");

                for (int j = 0; j < confusionMatrix.NumberOfClasses; j++)
                {
                    Console.Write(
                        string.Format(
                            "{0," + width + "}",
                            confusionMatrix.Counts[i][j]
                        )
                    );
                }

                Console.WriteLine();
            }


            // Per-class metrics
            Console.WriteLine($"\nPer-class metrics ({datasetName}):");

            Console.WriteLine(
                $"{"Class",-15} {"Precision",12} {"Recall",12} {"F1",12}"
            );


            for (int i = 0; i < confusionMatrix.NumberOfClasses; i++)
            {
                double precision = confusionMatrix.PerClassPrecision[i];
                double recall = confusionMatrix.PerClassRecall[i];

                double f1 = (precision + recall) == 0
                    ? 0
                    : 2 * precision * recall / (precision + recall);


                Console.WriteLine(
                    $"{labels[i],-15}" +
                    $"{precision,12:P2}" +
                    $"{recall,12:P2}" +
                    $"{f1,12:P2}"
                );
            }
        }
    }
}