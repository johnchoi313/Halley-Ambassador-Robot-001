using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoJointControl : MonoBehaviour {

	/* --- PRE SETUP --- */
	public float servoDisplaySpeed;
	static float servoSpeed;
    private float delay = 0;
  	public float wait;

	//Setup parameters to connect to Arduino
	private bool connected = false;
	private SerialPort serial;
	public int baudrate;
    public string port;

	//data receive and transmit variables
	public string strIn = "";        
	public string strOut = "";

	//unity joint class for each bone
	[System.Serializable]
	public class joint {
		//joint variables.
		public Transform rotator;
		private Vector3 defaultRot;
		public string axis; //either x y or z
		public int angle; public int minPos; public int maxPos;
		//function to get default angle.
		public void getDefaultAngle() {
			defaultRot = rotator.localEulerAngles;
    	}
		//function to get default angle.
		public void gotoDefaultAngle() {
			switch(axis) {
				case "x": angle = (int)rotator.localEulerAngles.x; break;
				case "y": angle = (int)rotator.localEulerAngles.y; break;
				case "z": angle = (int)rotator.localEulerAngles.z; break;
            }
        }
  	    //function to clamp the joint axis.
		public void clampJoint() {
			if(angle < minPos) {angle = minPos;}
			else if(angle > maxPos) {angle = maxPos;}
		}
		//function to update localEulerAngles
		public void updateEulerAngles(float timeDelta) {
			Vector3 dr = defaultRot;
			Vector3 rr = rotator.localEulerAngles;
			float newAngle = 0;

			switch(axis) {
				case "x": 
					newAngle = Mathf.Lerp(rr.x,angle*1f,timeDelta*servoSpeed);
	            	rotator.localEulerAngles = new Vector3(newAngle,dr.y,dr.z); 
					break;
				case "y": 
					newAngle = Mathf.Lerp(rr.y,angle*1f,timeDelta*servoSpeed );
					rotator.localEulerAngles = new Vector3(dr.x,newAngle,dr.z); 
					break;
				case "z": 
					newAngle = Mathf.Lerp(rr.z,angle*1f,timeDelta*servoSpeed);
					rotator.localEulerAngles = new Vector3(dr.x,dr.y,newAngle); 
					break;
			}
        }
        //function to compute what value to send.
		public string getServoPos() {
			return angle.ToString();
		}
	}
	//Unity joint data
	public joint[] joints;
	/* Joint Order, Name (axis, defaultPos, minPos, maxPos):
	 ***HEAD, NECK AND CHEST***
	 0 = Chest (z,90,70,110)
     1 = Neck Twist (y,90,30,150)
	 2 = Neck (x,50,1,90)
	 ***LEFT ARM***
     3 = Left clavical (x,90,1,179)
     4 = Left shoulder (z,179,1,179)
     5 = Left Arm Twist (y,90,1,179)
     6 = Left Elbow (z,135,80,179)
     ***RIGHT ARM***
     7 = Right clavical (x,90,1,179)
     8 = Right shoulder (z,10,1,179)
     9 = Right Arm Twist (y,90,1,179)
     10 = Right Elbow (x,45,1,100)
	 */

	//mouse control
	public static bool mouseControl = false;

	/* ----- SETUP ----- */
	void Start () {
		//Connect to Arduino via Serial.
		serial = new SerialPort(port, baudrate, Parity.None, 8, StopBits.One);
		serial.Open();
		connected = true;
		Debug.Log("Halley awake.");

		//get default angles
		for(int j = 0; j < joints.Length; j++) { 
			joints[j].getDefaultAngle(); 
			joints[j].gotoDefaultAngle();
		}

		//set servo display update speed:
		servoSpeed = servoDisplaySpeed;
    }

	/* --- MAIN LOOP --- */
	void Update() {
		//check that we are connected:
		if(connected && delay <= 0) {
			//reset the string to send.
			strOut = "";

			//check whether pose is set from poseControl:
			if(PoseControl.pose != null) {
				for(int i = 0; i < joints.Length; i++) {
					joints[i].angle = PoseControl.pose[i];
				}
				PoseControl.pose = null;
      		}

			//check whether mouse heed control = true:
			if(mouseControl == true) {
				joints[1].angle = (int)(Input.mousePosition.x / Screen.width * 
					(float)(joints[1].maxPos - joints[1].minPos)) + joints[1].minPos; 
				joints[2].angle = (int)(Input.mousePosition.y / Screen.height * 
					(float)(joints[2].maxPos - joints[2].minPos)) + joints[2].minPos; 
			}

			//for every joint...
			for(int j = 0; j < joints.Length; j++) {
				//clamp the joint to its defined constraints.
				joints[j].clampJoint();
				//add the Servo angle value of the joint to send. 
				strOut += joints[j].getServoPos() + ":";
			}
			//check what we are sending:
			Debug.Log(strOut);
      
			//when done, finish with a semicolon.
			strOut += ";";

			//Send data to Arduino
			if(strOut != null) {
			  serial.Write(strOut);
			}
			//reset the delay:
			delay = wait;
		} else if (delay > 0) {
			delay -= Time.deltaTime;
		}
		//update the model's eulerAngles
		for(int j = 0; j < joints.Length; j++) {
	    	joints[j].updateEulerAngles(Time.deltaTime);
		}
    }
  
    //On quitting, disconnect
	void OnApplicationQuit() { serial.Close(); }
}
