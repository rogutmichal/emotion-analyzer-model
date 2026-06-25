# EmotionModel – Model uczenia maszynowego do wykrywania emocji w tekstach przy użyciu LightGBM

##  Opis projektu

`EmotionModel` to projekt w języku C# wykorzystujący **ML.NET** do analizy tekstów i przewidywania emocji wyrażanych w recenzjach lub opiniach.  
Projekt trenuje model klasyfikacji wieloklasowej, który rozpoznaje sześć emocji:  

- sadness  
- anger  
- love  
- surprise  
- fear  
- joy  
Do treningu modelu wykorzystano zbiór danych: https://www.kaggle.com/datasets/praveengovi/emotions-dataset-for-nlp
Model może przewidzieć obecność emocji dla dowolnego tekstu oraz ocenić skuteczność klasyfikacji na danych walidacyjnych i testowych.

---

##  Funkcjonalności

- Ładowanie danych treningowych, walidacyjnych i testowych z plików `.txt`.  
- Trenowanie modelu klasyfikacji emocji przy użyciu **LightGBM** i reprezentacji n-gramów.  
- Prognozowanie emocji dla dowolnego tekstu.  
- Ewaluacja modelu: dokładność mikro/makro, log-loss, macierz pomyłek, skuteczność dla każdej klasy i ważona dokładność.  
- Obsługa wag dla rzadziej występujących emocji w danych treningowych.  

---

##  Jak uruchomić projekt

1. **Sklonuj repozytorium:**
```bash
git clone https://github.com/rogutmichal/emocje.git
```

2. **Otwórz projekt w Visual Studio** wybierając plik `emocje.sln`.

3. **Zainstaluj wymagane pakiety NuGet**:  
   - Microsoft.ML  
   - Microsoft.ML.LightGbm  

4. **Uruchom projekt**  
   - Jeśli model nie istnieje, zostanie wytrenowany automatycznie na danych z `train.txt`.  
   - Po wytrenowaniu model jest zapisany w `emotion_model.zip` i może być ponownie użyty.

5. **Prognozowanie emocji dla przykładowego tekstu** odbywa się w `Program.cs` 

---

##  Ewaluacja modelu

- Raporty zawierają dokładność mikro/makro, log-loss i macierz pomyłek.  
- Wyświetlana jest skuteczność dla każdej emocji.  
- Wyliczana jest również **ważona dokładność**, uwzględniająca liczbę próbek dla każdej emocji.

---

##  Jak działa model

1. **Przetwarzanie tekstu:** normalizacja, tokenizacja, usuwanie stop-words, tworzenie n-gramów (1-3).  
2. **Konwersja tokenów na wartości numeryczne**  
3. **Trenowanie LightGBM** na danych z uwzględnieniem wag dla niedoreprezentowanych emocji.  
4. **Predykcja:** model zwraca prawdopodobieństwa dla wszystkich 6 emocji 

---
Model osiągnął 90.40% dokładności na niezależnym zbiorze testowym (test.txt). Najlepiej rozpoznawane emocje to love (96.23%), surprise (93.94%) oraz anger (93.82%).

##  Wyniki modelu na zbiorze testowym (`test.txt`)

Model został oceniony wyłącznie na zbiorze testowym zawierającym **2000 próbek**.

### Główne metryki

| Metryka | Wynik |
|----------|--------|
| Accuracy (Micro) | **90.40%** |
| Accuracy (Macro) | **91.79%** |
| Log Loss | **0.2610** |
| Poprawne predykcje | **1808 / 2000** |

### Skuteczność dla poszczególnych emocji

| Emocja | Accuracy |
|---------|----------|
| anger | 93.82% (258/275) |
| fear | 88.39% (198/224) |
| joy | 88.49% (615/695) |
| love | 96.23% (153/159) |
| sadness | 89.85% (522/581) |
| surprise | 93.94% (62/66) |

### Precision, Recall i F1-Score

| Emocja | Precision | Recall | F1-Score |
|---------|-----------|--------|----------|
| sadness | 98.12% | 89.85% | 93.80% |
| anger | 88.97% | 93.82% | 91.33% |
| love | 70.83% | 96.23% | 81.60% |
| surprise | 63.27% | 93.94% | 75.61% |
| fear | 88.00% | 88.39% | 88.20% |
| joy | 96.24% | 88.49% | 92.20% |

<details>
<summary><b>Confusion Matrix (TEST)</b></summary>

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

