# EmotionModelPrediction – Machine Learning Model for Emotion Detection Using LightGBM

## Project Overview

**EmotionModel** is a C# project built with **ML.NET** that analyzes text and predicts emotions expressed in reviews or user-generated content.

The project trains a **multiclass classification** model capable of recognizing six emotions:

- sadness
- anger
- love
- surprise
- fear
- joy

The model was trained using the following Kaggle dataset:
https://www.kaggle.com/datasets/praveengovi/emotions-dataset-for-nlp

It can predict the dominant emotion in any input text and evaluate its performance on validation and test datasets.

---

## Features

- Load training, validation, and test datasets from `.txt` files.
- Handle class imbalance by applying weights to underrepresented emotions.
- Train an emotion classification model using **LightGBM** and **n-gram text features**.
- Predict emotions for any input text.
- Evaluate model performance using:
  - Micro and Macro Accuracy
  - Log Loss
  - Confusion Matrix
  - Per-class Accuracy
  - Weighted Accuracy

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/rogutmichal/emocje.git
```

### 2. Open the solution

Open `emocje.sln` in **Visual Studio**.

### 3. Install the required NuGet packages

- Microsoft.ML
- Microsoft.ML.LightGbm

### 4. Run the project

- If no trained model exists, it will automatically be trained using `train.txt`.
- After training, the model is saved as `emotion_model.zip` and can be reused in future runs.

### 5. Predict emotions

Example predictions can be found in `Program.cs`.

---

## Model Evaluation

The evaluation reports include:

- Micro Accuracy
- Macro Accuracy
- Log Loss
- Confusion Matrix
- Accuracy for each emotion class

---

## How the Model Works

1. **Text preprocessing**
   - Text normalization
   - Tokenization
   - Stop-word removal
   - Generation of 1–3 word n-grams

2. **Feature extraction**
   - Conversion of text into numerical feature vectors

3. **Model training**
   - LightGBM multiclass classifier
   - Class weighting to improve performance on underrepresented emotions

4. **Prediction**
   - The model outputs probabilities for all six emotion classes.

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
