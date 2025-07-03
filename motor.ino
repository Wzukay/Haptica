// Pin definition for PWM
const int pwmPin = D4; // GPIO2

// PWM parameters
const int pwmMin = 0;
const int pwmMax = 1023; // 10-bit PWM on ESP8266
const int step = 10;     // PWM step size
const int delayTime = 20; // Delay between steps (ms)

void setup() {
  pinMode(pwmPin, OUTPUT);
}

void loop() {
  // Gradually increase power
  for (int duty = pwmMin; duty <= pwmMax; duty += step) {
    analogWrite(pwmPin, duty);
    delay(delayTime);
  }

  // Gradually decrease power
  for (int duty = pwmMax; duty >= pwmMin; duty -= step) {
    analogWrite(pwmPin, duty);
    delay(delayTime);
  }
}
