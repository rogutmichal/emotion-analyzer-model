using EmotionAnalyzerAPI.Services;
using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.ML;


var builder =
    WebApplication.CreateBuilder(args);



builder.Configuration.Sources.Clear();


builder.Configuration
    .AddJsonFile(
        "appsettings.json",
        optional: false,
        reloadOnChange: false)
    .AddEnvironmentVariables();


// ===============================
// Controllers
// ===============================

builder.Services.AddControllers();


// ===============================
// Swagger
// ===============================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ===============================
// Configuration
// ===============================

builder.Services.Configure<ModelSettings>(
    builder.Configuration
        .GetSection("ModelSettings"));


// ===============================
// ML Services
// ===============================

// Jeden wspólny MLContext dla ca³ej aplikacji
builder.Services.AddSingleton<MLContext>();


// ===============================
// Model services
// ===============================

builder.Services.AddSingleton<ModelLoader>();

builder.Services.AddSingleton<LoadedModelService>();


// ===============================
// Training
// ===============================

builder.Services.AddSingleton<EmotionModelTrainer>();

builder.Services.AddSingleton<TrainingService>();


// ===============================
// Prediction
// ===============================
builder.Services.AddSingleton<ModelInitializer>();

builder.Services.AddSingleton<EmotionPredictionService>();


// ===============================
// Evaluation
// ===============================

builder.Services.AddSingleton<EvaluationStorageService>();
builder.Services.AddSingleton<ModelEvaluationService>();



var app = builder.Build();



// ===============================
// HTTP pipeline
// ===============================


    app.UseSwagger();

    app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var initializer =
        scope.ServiceProvider
            .GetRequiredService<ModelInitializer>();

    initializer.Initialize();
}

app.MapGet("/", () =>
{
    return "Emotion Analyzer API is running";
});


app.Run();