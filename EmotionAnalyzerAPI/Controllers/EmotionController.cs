using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmotionAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/emotion")]
    public class EmotionController : ControllerBase
    {
        private readonly EmotionPredictionService _predictionService;
        private readonly LoadedModelService _loadedModelService;

        public EmotionController(
            EmotionPredictionService predictionService,
            LoadedModelService loadedModelService)
        {
            _predictionService = predictionService;
            _loadedModelService = loadedModelService;
        }


        [HttpPost("predict")]
        public IActionResult Predict(
            [FromBody] PredictionRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest(new
                {
                    message = "Text is required."
                });
            }


            if (!_loadedModelService.IsLoaded)
            {
                return BadRequest(new
                {
                    message = "Model is not loaded."
                });
            }


            var result =
                _predictionService.Predict(
                    request.Text);

            return Ok(result);
        }
    }
}