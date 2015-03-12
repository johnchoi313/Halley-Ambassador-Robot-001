/* --- PRE SETUP --- */
#include <SoftwareSerial.h>
#include <Servo.h>

/* --- CONSTANTS --- */
const int servoSpeed = 4;
const int jointsLen = 11;
const int msgLen = 50;
const int wait = 50;

/* --- Joint Class --- */
class Joint {
  public:
    //Joint functions.
    Joint();
    void initialize(int _pin, int defaultPos, int _minPos, int _maxPos);
    void setPos(int angle);  
    void gotoDefaultPos();
    void updateServo();
    Servo servo;
  private:
    //Joint variables.
    int defaultPos;
    int minPos;
    int maxPos;
    int toPos;
    int pos;
    int pin;
};
Joint::Joint() {}
void Joint::initialize(int _pin, int _defaultPos, int _minPos, int _maxPos) {
  pin = _pin;
  minPos = _minPos;
  maxPos = _maxPos;
  servo.attach(pin);
  pos = _defaultPos;
  toPos = _defaultPos;
  defaultPos = _defaultPos;
}
void Joint::setPos(int angle) { 
  //clamp destination position
  if(angle < 0) {angle = 0;}
  else if(angle > 180) {angle = 180;}
  //set destination position
  toPos = angle; 
}
void Joint::updateServo() {
  //clamp toPos
  if(toPos > 180) { toPos = 180; }
  else if(toPos < 0) { toPos = 0; }
  //move pos to toPos
  if(pos < toPos && pos + servoSpeed <= toPos) { pos += servoSpeed; } 
  else if (pos > toPos && pos - servoSpeed >= toPos) { pos -= servoSpeed; }
  else {pos = toPos;}
  //clamp pos
  if(pos > 180) { pos = 180; }
  else if(pos < 0) { pos = 0; }
  //write position to servo
  servo.write(pos);
}
void Joint::gotoDefaultPos() {
  toPos = defaultPos;
}

//Make joints array.
Joint joints[10];
/* Joint order, Name (pin, defaultPos, minPos, maxPos):
 ***HEAD, NECK AND CHEST***
 0 = Chest (40,90,70,110)
 1 = Neck Twist (36,90,30,150)
 2 = Neck (34,50,0,90)
 ***LEFT ARM***
 3 = Left clavical (42,90,0,180)
 4 = Left shoulder (44,180,0,180)
 5 = Left Arm Twist (46,90,0,180)
 6 = Left Elbow (50,135,80,180)
 ***RIGHT ARM***
 7 = Right clavical (53,90,0,180)
 8 = Right shoulder (43,10,0,180)
 9 = Right Arm Twist (49,90,0,180)
 10 = Right Elbow (47,45,80,180)
 */

//Other Variables
boolean powerChest = true;
char angleData[msgLen];
String strIn = "";
char *angles;

/* ----- SETUP ----- */
void setup() {
  //prepare the servos.
  /***HEAD, NECK AND CHEST***/
  joints[0].initialize(40,90,70,110);
  joints[1].initialize(36,90,30,150);
  joints[2].initialize(34,50,0,90);
  /***LEFT ARM***/
  joints[3].initialize(42,90,0,180);
  joints[4].initialize(44,180,0,180);
  joints[5].initialize(46,90,0,180);
  joints[6].initialize(50,135,80,180);
  /***RIGHT ARM***/
  joints[7].initialize(53,90,0,180);
  joints[8].initialize(43,10,0,180);
  joints[9].initialize(49,90,0,180);
  joints[10].initialize(47,45,0,100);
 
  //begin serial
  Serial.begin(9600);
  Serial.println("Halley awake.");
}

/* --- MAIN LOOP --- */
void loop() {
  
  //get Serial data from Unity.
  if(Serial.available() > 0) {
    strIn = Serial.readStringUntil(';');
  }
  
  //convert the string to a char array.
  strIn.toCharArray(angleData,msgLen);
  
  //split the string into ints representing angles.
  angles = strtok(angleData , ":\n");
  int j = 0;
  while(angles != NULL) {
    if (j < jointsLen) {
      joints[j].setPos(atoi(angles)); 
    }
    angles = strtok(NULL,":\n");
    j++;
  }
 
  //write to the servo angles.
  for(int i = 1; i < jointsLen; i++) {
    //write to individual angle
    joints[i].updateServo();
  }
  //Force Chest.
  joints[0].servo.write(90);
  
  //wait a bit.
  delay(wait);
}

void allDefault() {
  for(int j = 0; j < jointsLen; j++) {
    joints[j].gotoDefaultPos();
  }
}

