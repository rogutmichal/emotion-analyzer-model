using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
using System.Linq;
using System.Text.RegularExpressions;
using emocje.Models;
using emocje;






public class EmotionBasedRecommendation
{
    // Ścieżki do plików danych i modelu
    private static readonly string ModelPath = "emotion_model.zip";
    private static readonly string TrainFilePath = "Data/train.txt";
    private static readonly string ValFilePath = "Data/val.txt";
    private static readonly string TestFilePath = "Data/test.txt";
    public static void Main(string[] args)
    {

        var context = new MLContext();
        // Wczytanie danych treningowych
        var trainTexts = DataLoader.LoadDataFromFile(TrainFilePath);
        ITransformer model;
        
        if (!File.Exists(ModelPath))
        {
            // Jeśli model nie istnieje, trenujemy go
            Console.WriteLine("Trenowanie modelu");
            model = EmotionModelTrainer.TrainModel(context, trainTexts);
            context.Model.Save(model, context.Data.LoadFromEnumerable(trainTexts).Schema, ModelPath);
        }
        else
        {
            // Jeśli model istnieje, wczytujemy go
            Console.WriteLine("Ładowanie modelu");
            model = context.Model.Load(ModelPath, out _);
        }

        Console.WriteLine("Wpisz tekst do analizy emocji:");
        string userReview = Console.ReadLine();
        // Przewidywanie emocji dla wprowadzonego tekstu
        var exampleText = new TextData { Text = userReview }; var Emotions = EmotionModel.PredictEmotion(context, model, exampleText);

        Console.WriteLine($"Recenzja: {exampleText.Text}");
        Console.WriteLine("Top emocje :");
        foreach (var kvp in Emotions)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value:P2}");
        }
        // Testowanie modelu na danych walidacyjnych i testowych
        var valReviews = DataLoader.LoadDataFromFile(ValFilePath);
        ModelEvaluator.TestModel(context, model, valReviews, "VALIDATION");

        var testReviews = DataLoader.LoadDataFromFile(TestFilePath);
        ModelEvaluator.TestModel(context, model, testReviews, "TEST");

        Console.ReadKey();
    }
}