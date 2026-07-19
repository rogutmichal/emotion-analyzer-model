using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Trainers.LightGbm;


namespace EmotionAnalyzerML.Services
{
   
    public class EmotionModelTrainer
    // This class is responsible for creating pipeline the ML.NET model
    {
        private readonly MLContext _context;


        public EmotionModelTrainer(
            MLContext context)
        {
            _context = context;
        }



        public ITransformer TrainModel(
            List<TextData> texts)
        {
            // Validate the input data
            if (texts == null || texts.Count == 0)
            {
                throw new ArgumentException(
                    "Training data cannot be empty.");
            }



            // Calculate the class counts for each emotion
            var classCounts =
                texts
                .GroupBy(x => x.Emotion)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count());


            
            int maxCount = classCounts.Values.Max();


            // Create a weighted dataset to handle class imbalance
            var weightedTexts =
                texts
                .Select(x => new WeightedData
                {
                    Text = x.Text,

                    Emotion = x.Emotion,

                    Weight =
                        (float)maxCount / classCounts[x.Emotion]

                })
                .ToList();



            // Load the weighted dataset into an IDataView
            var trainData =
                _context.Data
                .LoadFromEnumerable(
                    weightedTexts);


            // Cache the training data
            var cachedTrainData =
                _context.Data.Cache(
                    trainData);

            // Define the ML.NET pipeline
            var pipeline =
                _context.Transforms
                .Conversion
                .MapValueToKey(
                    "Label",
                    "Emotion")

                //Transform the text data into features for the model(Normalize, Tokenize, Remove Stop Words, Map to Keys)
                .Append(
                    _context.Transforms.Text
                    .NormalizeText(
                        "NormalizedText",
                        "Text"))

                .Append(
                    _context.Transforms.Text
                    .TokenizeIntoWords(
                        "Tokens",
                        "NormalizedText"))

                .Append(
                    _context.Transforms.Text
                    .RemoveDefaultStopWords(
                        "TokensClean",
                        "Tokens"))

                .Append(
                    _context.Transforms.Conversion
                    .MapValueToKey(
                        "TokensKeys",
                        "TokensClean"))

                // Produce N-grams for unigrams, bigrams, and trigrams

                .Append(
                    _context.Transforms.Text
                    .ProduceNgrams(
                        "UniGrams",
                        "TokensKeys",
                        ngramLength: 1,
                        useAllLengths: false))


                .Append(
                    _context.Transforms.Text
                    .ProduceNgrams(
                        "BiGrams",
                        "TokensKeys",
                        ngramLength: 2,
                        useAllLengths: false))


                .Append(
                    _context.Transforms.Text
                    .ProduceNgrams(
                        "TriGrams",
                        "TokensKeys",
                        ngramLength: 3,
                        useAllLengths: false))

                 .Append(
                    _context.Transforms
                    .Concatenate(
                        "Features",
                        "UniGrams",
                        "BiGrams",
                        "TriGrams"))

                // Train the model using LightGBM with specified hyperparameters
                .Append(
                    _context.MulticlassClassification
                    .Trainers
                    .LightGbm(
                        new LightGbmMulticlassTrainer.Options
                        {
                            LabelColumnName = "Label",
                            FeatureColumnName = "Features",
                            ExampleWeightColumnName = "Weight",                         
                            NumberOfLeaves = 104,
                            MinimumExampleCountPerLeaf = 5,
                            LearningRate = 0.006,
                            NumberOfIterations = 875
                        }))
                // Map the predicted label back to the original emotion string
                .Append(
                    _context.Transforms
                    .Conversion
                    .MapKeyToValue(
                        "PredictedEmotion",
                        "PredictedLabel"));

            return pipeline.Fit(
                cachedTrainData);
        }
    }
}