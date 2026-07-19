using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionAnalyzerML.Models
{
    //text data class for the model input
    public class TextData
    {
        public string Text { get; set; }
        public string Emotion { get; set; }


    }
}