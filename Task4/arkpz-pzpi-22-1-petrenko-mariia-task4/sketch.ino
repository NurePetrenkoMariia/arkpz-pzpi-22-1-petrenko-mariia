#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <TimeLib.h>

#define TRIG_PIN 5 //Пін на платі для відправки ультразвукового імпульсу
#define ECHO_PIN 18 //Пін на платі для приймання відбитого сигналу 

float speedOfSound = 0.034;

DynamicJsonDocument feedHistoryDoc(4096); //Документ для історії рівня корму
JsonArray feedHistory = feedHistoryDoc.to<JsonArray>(); // Масив з історією рівня корму

String serverBaseUrl;
String stableId;
int feederHeight;
String wifiName;
String wifiPassword;

void setup() {
  Serial.begin(115200);

  pinMode(TRIG_PIN, OUTPUT); //Налаштування для відправки сигналу
  pinMode(ECHO_PIN, INPUT); //Налаштування для прийому сигналу

  configureDevice(); //Виклик функції налаштування пристрою

  WiFi.begin(wifiName.c_str(), wifiPassword.c_str()); //Підключення до вай-фай
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi successfully.");
}

//Функція налаштування
void configureDevice() {
  Serial.println("Device settings");

  Serial.print("Enter WiFi Name: ");
  wifiName = readSerialInput();
  Serial.println(wifiName);

  Serial.print("Enter WiFi Password: ");
  wifiPassword = readSerialInput();
  Serial.println("WiFi Password set.");

  Serial.print("Enter Server Base URL: ");
  serverBaseUrl = readSerialInput();
  Serial.println(serverBaseUrl);

  Serial.print("Enter Stable ID: ");
  stableId = readSerialInput();
  Serial.println(stableId);

  Serial.print("Enter feeder height: ");
  feederHeight = readSerialInput().toInt();
  Serial.println(String(feederHeight));
}

//Функція зчитування вводу користувача
String readSerialInput() {
  String input = "";
  while (true) {
    while (Serial.available() > 0) {
      char c = Serial.read();
      if (c == '\n') {
        return input;
      } else {
        input += c;
      }
    }
  }
}

//Функція для визначення рівня корму
float measureFeedLevel() {
  digitalWrite(TRIG_PIN, LOW); //Початок вимірювання з 0 значення
  delayMicroseconds(2);
  digitalWrite(TRIG_PIN, HIGH); //Надсилання імпульсу
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW); //Завершення вимірювання

  long duration = pulseIn(ECHO_PIN, HIGH); //Вимір часу між надсиланням та поверненням імпульсу
  float distance = duration * speedOfSound / 2; //Розрахунок відстані в см

  return feederHeight - round(distance);
}

//Конвертація часу у тип time_t
time_t parseISO8601(String timestamp) {
  tmElements_t tm;

  tm.Year = timestamp.substring(0, 4).toInt() - 1970;
  tm.Month = timestamp.substring(5, 7).toInt();
  tm.Day = timestamp.substring(8, 10).toInt();
  tm.Hour = timestamp.substring(11, 13).toInt();
  tm.Minute = timestamp.substring(14, 16).toInt();
  tm.Second = timestamp.substring(17, 19).toInt();

  return makeTime(tm);
}

//Функція для розрахунку швидкості споживання корму
float calculateFeedConsumptionSpeed(const JsonArray& feedHistory) {
  float totalConsumption = 0.0;
  long totalTime = 0;

  for (size_t i = 1; i < feedHistory.size(); i++) {
    float prevLevel = feedHistory[i - 1]["feedLevel"]; //Попередній рівень корму
    float currLevel = feedHistory[i]["feedLevel"]; //Поточний рівень корму
    time_t prevTime = parseISO8601(feedHistory[i - 1]["timestamp"].as<String>());
    time_t currTime = parseISO8601(feedHistory[i]["timestamp"].as<String>());
    totalConsumption += (prevLevel - currLevel);
    totalTime += (currTime - prevTime);
  }

  float speed = totalConsumption / (totalTime / 3600.0); //Обчислення кількості спожитого корму за годину
  Serial.println("Calculated consumption speed: " + String(speed));
  return speed;
}

//Функція для прогнозування часу закінчення корму
long predictTimeToEmpty(int currentLevel, float consumptionSpeed) {
  if (consumptionSpeed <= 0) {
    return -1;
  }
  return (currentLevel / consumptionSpeed) * 3600;
}

//Функція надсилання даних на сервер
void sendFeedDataToServer(int feedLevel, long timeToEmpty) {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http; //Об'єкт для виконання запитів
    String url = serverBaseUrl + "FeedMonitoring/update-feed-level";
    http.begin(url);
    http.addHeader("Content-Type", "application/json"); //Налаштування формату даних

    String payload = "{\"StableId\": \"" + stableId +
                     "\", \"CurrentFeedLevel\": " + String(feedLevel) +
                     (timeToEmpty > 0 ? ", \"PredictedTimeToEmpty\": " + String(timeToEmpty) : "") + "}";

    int httpCode = http.POST(payload); //Надсилання на сервер
    if (httpCode > 0) {
      Serial.println("Data sent successfully.");
    } else {
      Serial.println("Error sending data to server.");
    }
    http.end();
  } else {
    Serial.println("WiFi disconnected!");
  }
}

//Функція отримання даних з сервера
void fetchFeedHistory() {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    String url = serverBaseUrl + "FeedLevelHistory/get-feed-history/" + stableId;
    http.begin(url);
    int httpCode = http.GET();

    //Якщо дані отримані успішно
    if (httpCode == 200) {
      String response = http.getString();

      //Десеріалізація y JSON в об'єкт feedHistoryDoc
      DeserializationError error = deserializeJson(feedHistoryDoc, response);

      if (error) {
        Serial.println("JSON Deserialization Error: " + String(error.c_str()));
        http.end();
        return;
      }

      //Перевірка структури JSON
      if (feedHistoryDoc.is<JsonArray>()) {
        feedHistory = feedHistoryDoc.as<JsonArray>();
        Serial.println("Feed History Size: " + String(feedHistory.size()));
      } else {
        Serial.println("Unexpected JSON structure.");
      }
    } else {
      Serial.println("HTTP Error: " + String(httpCode));
    }
    http.end();
  } else {
    Serial.println("WiFi disconnected!");
  }
}

void loop() {
  int currentFeedLevel = measureFeedLevel(); //Вимірювання рівня корму
  Serial.println("Measured feed level: " + String(currentFeedLevel));

  fetchFeedHistory(); //Отримання даних з сервера

  if (feedHistory.size() >= 2) {
    float consumptionSpeed = calculateFeedConsumptionSpeed(feedHistory); //Рохрахунок швидкості споживання
    long timeToEmpty = -1;

    if (consumptionSpeed > 0) {
      timeToEmpty = predictTimeToEmpty(currentFeedLevel, consumptionSpeed); //Розрахунок прогнозу
      Serial.println("Predicted time to empty (s): " + String(timeToEmpty));
    } else {
      Serial.println("Not enough data to predict time to empty.");
    }

    sendFeedDataToServer(currentFeedLevel, timeToEmpty); //Надсилання на сервер
  } else {
    Serial.println("Not enough records in feed history.");
  }

  delay(100000); //Затримка між вимірюваннями
}
