using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionAnalyzerML.Models
{
    public class WeightedData : TextData
    {
        public float Weight { get; set; }
    }
}