namespace EmotionAnalyzerML.Models
{
    public class EmotionPredictionResult
    // Represents the result of an emotion prediction
    {


        public string Text { get; set; }


        public List<EmotionScore> Predictions { get; set; }
    }


    public class EmotionScore
    {
        
        public string Emotion { get; set; }


        public float Confidence { get; set; }
    }
}