#define PULSE_PIN A0

const int threshold = 540;       // Fine-tuned threshold (adjust if needed)
bool pulseDetected = false;

unsigned long lastBeatTime = 0;
unsigned long lastPrintTime = 0;

#define MAX_BPM_READINGS 15
int bpmBuffer[MAX_BPM_READINGS];
int bpmIndex = 0;
int bpmSum = 0;
int bpmCount = 0;

void setup() {
  Serial.begin(115200);
  delay(1000);
  Serial.println("Heart rate monitor started");
}

void loop() {
  int signal = analogRead(PULSE_PIN);
  unsigned long now = millis();

  // Detect rising edge of pulse
  if (signal > threshold && !pulseDetected) {
    pulseDetected = true;

    if (lastBeatTime > 0) {
      unsigned long interval = now - lastBeatTime;

      // Only accept beats between 500–1500 ms (40–120 BPM)
      if (interval > 450 && interval < 900) {
        int bpm = 60000 / interval;

        // Rolling average
        bpmSum -= bpmBuffer[bpmIndex];
        bpmBuffer[bpmIndex] = bpm;
        bpmSum += bpm;
        bpmIndex = (bpmIndex + 1) % MAX_BPM_READINGS;
        if (bpmCount < MAX_BPM_READINGS) bpmCount++;
      }
    }

    lastBeatTime = now;
  }

  // Falling edge: reset detector
  if (signal < threshold - 30) {
    pulseDetected = false;
  }

  // Print BPM every 3 seconds
  if (now - lastPrintTime >= 3000 && bpmCount > 0) {
    int avgBPM = bpmSum / bpmCount;
    Serial.print("Average BPM: ");
    Serial.println(avgBPM);
    lastPrintTime = now;
  }

  delay(5);  // Small delay for stability
}
