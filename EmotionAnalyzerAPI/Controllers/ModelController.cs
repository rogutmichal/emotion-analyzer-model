using EmotionAnalyzerML.Data;
using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EmotionAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/model")]
    public class ModelController : ControllerBase
    {
        private readonly TrainingService _trainingService;
        private readonly LoadedModelService _loadedModelService;
        private readonly ModelLoader _modelLoader;
        private readonly ModelSettings _modelSettings;
        private readonly ModelEvaluationService _evaluationService;


        public ModelController(
            TrainingService trainingService,
            LoadedModelService loadedModelService,
            ModelLoader modelLoader,
            ModelEvaluationService evaluationService,
            IOptions<ModelSettings> options)
        {
            _trainingService = trainingService;
            _loadedModelService = loadedModelService;
            _modelLoader = modelLoader;
            _evaluationService = evaluationService;
            _modelSettings = options.Value;
        }



        [HttpPost("train")]
        public IActionResult Train()
        {
            try
            {
                var trainData =
                    DataLoader.LoadDataFromFile(
                        _modelSettings.TrainFilePath);


                _trainingService.TrainAndSave(
                    trainData,
                    _modelSettings.ModelPath);


                return Ok(new
                {
                    message = "Model trained successfully",
                    modelPath = _modelSettings.ModelPath
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Training failed",
                    error = ex.Message
                });
            }
        }




        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(new
            {
                loaded = _loadedModelService.IsLoaded,
                message = _loadedModelService.IsLoaded
                    ? "Model is ready"
                    : "Model is not loaded"
            });
        }





        [HttpGet("evaluate")]
        public IActionResult Evaluate()
        {
            try
            {
                if (!_loadedModelService.IsLoaded)
                {
                    return BadRequest(new
                    {
                        message = "Model is not loaded"
                    });
                }



                var testData =
                    DataLoader.LoadDataFromFile(
                        _modelSettings.TestFilePath);



                var result =
                    _evaluationService.Evaluate(
                        _loadedModelService.GetModel(),
                        testData,
                        "TEST");



                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Evaluation failed",
                    error = ex.Message
                });
            }
        }
    }
}