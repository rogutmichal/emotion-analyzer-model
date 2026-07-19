using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace EmotionAnalyzerML.Services
{
    //The service is responsible for the model evaluation
    public class ModelEvaluationService
    {
        private readonly MLContext _context;


        public ModelEvaluationService(
            MLContext context)
        {
            _context = context;
        }


        // Evaluates the provided model using the given test data and returns the evaluation results.
        public ModelEvaluationResult Evaluate(
            ITransformer model,
            List<TextData> texts,
            string datasetName)
        {

            var testData =
                _context.Data.LoadFromEnumerable(
                    texts);

            // Mapping the "Emotion" column to a key type for evaluation
            var labelMapping =
                _context.Transforms
                .Conversion
                .MapValueToKey(
                    "Label",
                    "Emotion");

            // Transform the test data
            var testDataWithLabel =
                labelMapping
                .Fit(testData)
                .Transform(testData);


            // Use the model to make predictions on the test data
            var predictions =
                model.Transform(
                    testDataWithLabel);


            // Evaluate the predictions using multiclass classification metrics
            var metrics =
                _context.MulticlassClassification
                .Evaluate(
                    predictions,
                    labelColumnName: "Label");



            var labelBuffer =
                new VBuffer<ReadOnlyMemory<char>>();

            // Extract the predicted labels from the predictions
            predictions.Schema["PredictedLabel"]
                .GetKeyValues(
                    ref labelBuffer);

           
            var labels =
                labelBuffer
                .DenseValues()
                .Select(x => x.ToString())
                .ToList();


            // Get the confusion matrix from the evaluation metrics
            var confusionMatrix =
                metrics.ConfusionMatrix;


            // Calculate precision, recall, and F1 score for each class
            var precision =
                new Dictionary<string, double>();

            var recall =
                new Dictionary<string, double>();

            var f1 =
                new Dictionary<string, double>();


            // Loop through each label and calculate the metrics
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


            // Return the evaluation results
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