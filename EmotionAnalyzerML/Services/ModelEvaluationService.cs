using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace EmotionAnalyzerML.Services
{
    public class ModelEvaluationService
    {
        private readonly MLContext _context;


        public ModelEvaluationService(
            MLContext context)
        {
            _context = context;
        }



        public ModelEvaluationResult Evaluate(
            ITransformer model,
            List<TextData> reviews,
            string datasetName)
        {

            var testData =
                _context.Data.LoadFromEnumerable(
                    reviews);


            // Dodanie Label
            var labelMapping =
                _context.Transforms
                .Conversion
                .MapValueToKey(
                    "Label",
                    "Emotion");


            var testDataWithLabel =
                labelMapping
                .Fit(testData)
                .Transform(testData);



            var predictions =
                model.Transform(
                    testDataWithLabel);



            var metrics =
                _context.MulticlassClassification
                .Evaluate(
                    predictions,
                    labelColumnName: "Label");



            // Pobranie nazw klas
            var labelBuffer =
                new VBuffer<ReadOnlyMemory<char>>();


            predictions.Schema["PredictedLabel"]
                .GetKeyValues(
                    ref labelBuffer);


            var labels =
                labelBuffer
                .DenseValues()
                .Select(x => x.ToString())
                .ToList();



            var confusionMatrix =
                metrics.ConfusionMatrix;



            var precision =
                new Dictionary<string, double>();

            var recall =
                new Dictionary<string, double>();

            var f1 =
                new Dictionary<string, double>();


            for (int i = 0; i < labels.Count; i++)
            {
                var p =
                    confusionMatrix
                    .PerClassPrecision[i];


                var r =
                    confusionMatrix
                    .PerClassRecall[i];


                var f =
                    (p + r) == 0
                    ? 0
                    : 2 * p * r / (p + r);



                precision[labels[i]] = p;

                recall[labels[i]] = r;

                f1[labels[i]] = f;
            }



            return new ModelEvaluationResult
            {
                DatasetName = datasetName,


                MicroAccuracy =
                    metrics.MicroAccuracy,


                MacroAccuracy =
                    metrics.MacroAccuracy,


                LogLoss =
                    metrics.LogLoss,


                Labels = labels,


                ConfusionMatrix =
                    confusionMatrix.Counts
                    .Select(row =>
                        row.Select(x => (long)x)
                        .ToArray())
                    .ToArray(),


                Precision = precision,

                Recall = recall,

                F1Score = f1
            };
        }
    }
}