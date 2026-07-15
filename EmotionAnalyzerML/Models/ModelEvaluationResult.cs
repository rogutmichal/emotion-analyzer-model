using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionAnalyzerML.Models
{
    public class ModelEvaluationResult
    {
        public string DatasetName { get; set; }


        // Ogólne metryki
        public double MicroAccuracy { get; set; }

        public double MacroAccuracy { get; set; }

        public double LogLoss { get; set; }


        // Macierz pomyłek
        public List<string> Labels { get; set; }

        public long[][] ConfusionMatrix { get; set; }


        // Metryki dla klas
        public Dictionary<string, double> Precision { get; set; }

        public Dictionary<string, double> Recall { get; set; }

        public Dictionary<string, double> F1Score { get; set; }
    }
}