using EmotionAnalyzerAPI.Services;
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
        private readonly EvaluationStorageService _storage;


        public ModelController(
            TrainingService trainingService,
            LoadedModelService loadedModelService,
            ModelLoader modelLoader,
            IOptions<ModelSettings> options,
            EvaluationStorageService storage)
        {
            _trainingService = trainingService;
            _loadedModelService = loadedModelService;
            _modelLoader = modelLoader;
            _modelSettings = options.Value;
            _storage = storage;
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





        [HttpPost("evaluate")]
        public IActionResult Evaluate()
        {
            try
            {
                if (!_loadedModelService.IsLoaded)
                {
                    return BadRequest(
                        "Model is not loaded.");
                }


                var testData =
                    DataLoader.LoadDataFromFile(
                        _modelSettings.TestFilePath);


                var evaluator =
                    new ModelEvaluationService(
                        new Microsoft.ML.MLContext());


                var result =
                    evaluator.Evaluate(
                        _loadedModelService.Model,
                        testData,
                        "TEST");


                _storage.Save(result);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("evaluation")]
        public IActionResult GetEvaluation()
        {
            var result =
                _storage.Load();


            if (result == null)
            {
                return NotFound(
                    new
                    {
                        message =
                        "No evaluation results available."
                    });
            }


            return Ok(result);
        }
    }
    }
