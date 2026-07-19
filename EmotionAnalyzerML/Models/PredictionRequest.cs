namespace EmotionAnalyzerML.Models
{
    public class PredictionRequest
    {
        // The text to be analyzed for emotion
        public string Text { get; set; } = string.Empty;
    }
}