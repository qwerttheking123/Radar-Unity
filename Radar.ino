
const int upbutton = 3;     
const int downbutton = 4;
const int leftbutton = 5;
const int rightbutton = 6;
const int guessbutton = 7; 
const int relay = 8 ;
int upbuttonold = 0;
int downbuttonold = 0;
int leftbuttonold = 0;
int rightbuttonold = 0;
int guessbuttonold = 0;
int relayon;

void setup() {
  pinMode(upbutton, INPUT_PULLUP);
  pinMode(downbutton, INPUT_PULLUP);
  pinMode(leftbutton, INPUT_PULLUP);
  pinMode(rightbutton, INPUT_PULLUP);
  pinMode(guessbutton, INPUT_PULLUP);
  pinMode(relay, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  if (digitalRead(upbutton) == LOW && upbuttonold == 0) {
    Serial.println("up");
    upbuttonold = 1;
  } else if (digitalRead(upbutton) == HIGH) {
    upbuttonold = 0;
  }
  if (digitalRead(downbutton) == LOW && downbuttonold == 0) {
    Serial.println("down");
    downbuttonold = 1;
  } else if (digitalRead(downbutton) == HIGH) {
    downbuttonold = 0;
  }
  if (digitalRead(leftbutton) == LOW && leftbuttonold == 0) {
    Serial.println("left");
    leftbuttonold = 1;
  } else if (digitalRead(leftbutton) == HIGH) {
    leftbuttonold = 0;
  }
  if (digitalRead(rightbutton) == LOW && rightbuttonold == 0) {
    Serial.println("right");
    rightbuttonold = 1;
  } else if (digitalRead(rightbutton) == HIGH) {
    rightbuttonold = 0;
  }
  if (digitalRead(guessbutton) == LOW && guessbuttonold == 0) {
    Serial.println("guess");
    guessbuttonold = 1;
  } else if (digitalRead(guessbutton) == HIGH) {
    guessbuttonold = 0;
  }
  if(Serial.available()){
    while (Serial.available() > 0) {
      relayon = Serial.read();
    }
    digitalWrite(relay, HIGH);
  }
  Serial.println(0);
  delay(50);
}
