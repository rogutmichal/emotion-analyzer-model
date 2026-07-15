using EmotionAnalyzerML;
using EmotionAnalyzerML.Services;
using Microsoft.ML;
using Microsoft.Extensions.Configuration;
using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Data;


public class EmotionBasedRecommendation
{
    public static void Main(string[] args)
    {
        var configuration =
            new ConfigurationBuilder()
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .Build();



        var modelSettings =
            configuration
                .GetSection("ModelSettings")
                .Get<ModelSettings>();



        var context = new MLContext();



        while (true)
        {
            Console.Clear();

            Console.WriteLine("==============================");
            Console.WriteLine("     Emotion Analyzer");
            Console.WriteLine("==============================");
            Console.WriteLine();

            Console.WriteLine("1. Text analysis");
            Console.WriteLine("2. Evaluation");
            Console.WriteLine("3. Model training");
            Console.WriteLine("0. Exit");

            Console.WriteLine();

            Console.Write("Select options: ");

            var choice = Console.ReadLine();



            switch (choice)
            {
                case "1":
                    PredictEmotion(
                        context,
                        modelSettings); 
                    break;


                case "2":
                    EvaluateModel(
                        context,
                        modelSettings);
                    break;


                case "3":
                    TrainModel(
                        context,
                        modelSettings); 
                    break;


                case "0":
                    return;


                default:
                    Console.WriteLine(
                        "Invalid option.");
                    break;
            }


            Console.WriteLine();

            Console.WriteLine(
                "Press any key to continue...");

            Console.ReadKey();
        }
    }





    private static void PredictEmotion(
        MLContext context,
        ModelSettings modelSettings)
    {
        Console.WriteLine();
        Console.WriteLine("=== Text analysis ===");



        if (!File.Exists(
            modelSettings.ModelPath)) 
        {
            Console.WriteLine(
                "Model not found.");

            Console.WriteLine(
                "Do the training first.");

            return;
        }



        var model =
            ModelLoader.Load(
                context,
                modelSettings.ModelPath); 


        var emotionService =
            new EmotionPredictionService(
                context,
                model);



        Console.Write("Enter text: ");

        var text = Console.ReadLine();



        var result =
            emotionService.Predict(text);



        Console.WriteLine();

        Console.WriteLine(
            $"Text: {result.Text}");

        Console.WriteLine();

        Console.WriteLine("Result:");



        foreach (var prediction in result.Predictions)
        {
            Console.WriteLine(
                $"{prediction.Emotion}: {prediction.Confidence:P2}");
        }
    }






    private static void EvaluateModel(
        MLContext context,
        ModelSettings modelSettings) 
    {
        Console.WriteLine();

        Console.WriteLine(
            "=== Model evaluation ===");



        if (!File.Exists(
            modelSettings.ModelPath)) 
        {
            Console.WriteLine(
                "Model not found.");

            return;
        }



        var model =
            ModelLoader.Load(
                context,
                modelSettings.ModelPath); 



        var testData =
            DataLoader.LoadDataFromFile(
                modelSettings.TestFilePath);



        var evaluator =
            new ModelEvaluationService(
                context);



        var result =
            evaluator.Evaluate(
                model,
                testData,
                "TEST");



        Console.WriteLine();

        Console.WriteLine(
            $"Micro Accuracy: {result.MicroAccuracy:P2}");

        Console.WriteLine(
            $"Macro Accuracy: {result.MacroAccuracy:P2}");

        Console.WriteLine(
            $"LogLoss: {result.LogLoss:F4}");



        Console.WriteLine();

        Console.WriteLine(
            "=== Per-class metrics ===");



        Console.WriteLine(
            $"{"Label",-15}" +
            $"{"Precision",15}" +
            $"{"Recall",15}" +
            $"{"F1-Score",15}");



        Console.WriteLine(
            new string('-', 60));



        foreach (var label in result.Labels)
        {
            Console.WriteLine(
                $"{label,-15}" +
                $"{result.Precision[label],15:P2}" +
                $"{result.Recall[label],15:P2}" +
                $"{result.F1Score[label],15:P2}");
        }



        Console.WriteLine();

        Console.WriteLine(
            "=== Confusion Matrix ===");


        int width = 12;


        Console.Write(
            string.Format(
                "{0,-15}",
                "Actual \\ Pred"));



        foreach (var label in result.Labels)
        {
            Console.Write(
                string.Format(
                    "{0," + width + "}",
                    label));
        }


        Console.WriteLine();



        for (int i = 0; i < result.Labels.Count; i++)
        {
            Console.Write(
                string.Format(
                    "{0,-15}",
                    result.Labels[i]));


            for (int j = 0; j < result.Labels.Count; j++)
            {
                Console.Write(
                    string.Format(
                        "{0," + width + "}",
                        result.ConfusionMatrix[i][j]));
            }


            Console.WriteLine();
        }
    }






    private static void TrainModel(
        MLContext context,
        ModelSettings modelSettings) 
    {
        Console.WriteLine();

        Console.WriteLine(
            "=== Model training ===");



        var trainData =
            DataLoader.LoadDataFromFile(
                modelSettings.TrainFilePath); 



        var trainer =
            new TrainingService(
                context);



        trainer.TrainAndSave(
            trainData,
            modelSettings.ModelPath); 



        Console.WriteLine();

        Console.WriteLine(
            "The model has been saved.");
    }
}