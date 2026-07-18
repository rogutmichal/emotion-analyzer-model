# Emotion Analyzer AI

## Machine Learning Text Emotion Classification Application

Emotion Analyzer AI is a full-stack application that uses **ML.NET** to analyze text and predict emotional states.

The system allows users to enter any text and receive probability scores for six different emotions.

The project consists of:

- **Blazor Web App** - user interface
- **ASP.NET Core Web API** - backend API
- **ML.NET Machine Learning Model** - emotion prediction engine
- **Docker** - containerization
- **Render** - cloud deployment


---

# Live Demo

Frontend:
https://emotion-analyzer-web.onrender.com/




---

# Features

## User Interface

- Modern Blazor Web App interface
- Text emotion analysis page
- Real-time prediction results
- Emotion probability visualization
- Responsive design


## Machine Learning

The model recognizes six emotions:

- sadness
- anger
- love
- surprise
- fear
- joy


The prediction returns:

- detected emotions
- confidence percentage
- probability ranking


---

# Application Architecture


```
User
 |
 |
Blazor Web App
 |
 |
ASP.NET Core API
 |
 |
ML.NET Model
 |
 |
LightGBM Classifier
 |
 |
Emotion Prediction
```


---

# How It Works


1. User enters text in the Blazor application

2. Frontend sends request to ASP.NET API

3. API passes text to the ML.NET prediction service

4. Machine learning model analyzes the text

5. The application returns probability scores for every emotion


Example:

Input:

```
I was nervous at first, but after seeing the results I felt incredibly happy.
```


Output:

```
joy        85.20%
fear       8.10%
surprise   4.20%
sadness    1.50%
```


---

# Machine Learning Model


The model was trained using the Kaggle dataset:

https://www.kaggle.com/datasets/praveengovi/emotions-dataset-for-nlp


Model:

- ML.NET
- LightGBM multiclass classifier
- Text featurization
- n-gram extraction
- class weighting


---

# Model Pipeline


```
Raw Text

↓

Text normalization

↓

Tokenization

↓

N-Gram Feature Extraction

↓

Numerical Feature Vector

↓

LightGBM Classification

↓

Emotion Probabilities
```


---

## Model Performance

The model achieved **90.40% accuracy** on an independent test dataset (`test.txt`).

The best-performing emotion classes were:

- **love** – 96.23%
- **surprise** – 93.94%
- **anger** – 93.82%

---

# Test Results (`test.txt`)

The model was evaluated on an independent test dataset containing **2,000 samples**.

## Overall Metrics

| Metric | Result |
|---------|--------|
| Micro Accuracy | **90.40%** |
| Macro Accuracy | **91.79%** |
| Log Loss | **0.2610** |
| Correct Predictions | **1808 / 2000** |

---

## Per-Class Accuracy

| Emotion | Accuracy |
|---------|----------|
| anger | 93.82% (258/275) |
| fear | 88.39% (198/224) |
| joy | 88.49% (615/695) |
| love | 96.23% (153/159) |
| sadness | 89.85% (522/581) |
| surprise | 93.94% (62/66) |

---

## Precision, Recall & F1-Score

| Emotion | Precision | Recall | F1-Score |
|---------|-----------|--------|----------|
| sadness | 98.12% | 89.85% | 93.80% |
| anger | 88.97% | 93.82% | 91.33% |
| love | 70.83% | 96.23% | 81.60% |
| surprise | 63.27% | 93.94% | 75.61% |
| fear | 88.00% | 88.39% | 88.20% |
| joy | 96.24% | 88.49% | 92.20% |

<details>
<summary><b>Confusion Matrix (Test Set)</b></summary>

```text
Predicted →    sadness     anger      love  surprise      fear       joy
sadness            522        18         4         4        14        19
anger                6       258         1         2         6         2
love                 0         2       153         1         0         3
surprise             0         0         0        62         4         0
fear                 1         6         1        18       198         0
joy                  3         6        57        11         3       615
```

</details>


---

# Technologies


## Backend

- C#
- ASP.NET Core 8
- REST API
- Dependency Injection


## Machine Learning

- ML.NET
- LightGBM
- Text Classification


## Frontend

- Blazor Web App
- Razor Components
- CSS


## Deployment

- Docker
- Render
- GitHub


---

# Running Locally


## Clone repository

```bash
git clone https://github.com/rogutmichal/emotion-analyzer-model.git
```


## Open solution

```
EmotionAnalyzer.sln
```


Projects:

```
EmotionAnalyzerAPI
EmotionAnalyzerML
EmotionAnalyzerWeb
EmotionAnalyzerConsole
```


---

# Run API

Navigate to:

```
EmotionAnalyzerAPI
```


Run:

```bash
dotnet run
```


API will start:

```
https://localhost:xxxx
```


---

# Run Frontend

Navigate to:

```
EmotionAnalyzerWeb
```


Run:

```bash
dotnet run
```


Open:

```
https://localhost:xxxx
```


---

# Docker Deployment


The API is containerized using Docker.


Docker build:

```bash
docker build -t emotion-analyzer-api .
```


The application is deployed using Render.


---




