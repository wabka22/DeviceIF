#include <Arduino.h>

#include <XSpaceBioV10.h>

const int ad8232Pin = 2;

  

void setup() {
Serial.begin(115200);

pinMode(ad8232Pin, INPUT);

}

  

void loop() {

int sensorValue = analogRead(ad8232Pin);

Serial.println(sensorValue);

delay(10); 
}