using UnityEngine;
using System.Collections;

public class PoseControl : MonoBehaviour {

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

    /*list of poses 
     * [Chest, Neck Twist, Neck, 
	 *  Left Clavical, Left Shoulder, Left Arm Twist, Left Elbow 
	 *  Right Clavical, Right Shoulder, Right Arm Twist, Right Elbow] */
	public int poseNum; //0-9
	[System.Serializable]
	public class Poses {
		public string poseName; //just for reference in editor
		public int[] pose;
	}
	public Poses[] poses;
	public static int[] pose;
	public static int face = 2;

	/* Pose information:
	 * Reset Pose = [90,90,50,90,179,90,120,90,10,90,45]
	 * Raise right hand = [90,90,50,90,179,90,135,90,170,170,10]
	 * Raise left hand = [90,90,50,90,10,1,170,90,1,90,45]; 
     * Shrug = [90,90,50,60,160,10,120,120,30,170,45]
	 * Hands Together  = [90,90,50,30,170,150,110,140,10,35,45]
     * Hands Up = [90,90,50,90,10,30,150,90,170,150,10]
	 */

	//mouse control
	public bool mouseControl;

	//initilization
	void Start () { }
	
	//update
	void Update () {
		//change the pose number by pressing the number keys
		if (Input.GetKeyDown(KeyCode.Keypad0)) { setPoseNum(0); }
		if (Input.GetKeyDown(KeyCode.Keypad1)) { setPoseNum(1); }
		if (Input.GetKeyDown(KeyCode.Keypad2)) { setPoseNum(2); }
		if (Input.GetKeyDown(KeyCode.Keypad3)) { setPoseNum(3); }
		if (Input.GetKeyDown(KeyCode.Keypad4)) { setPoseNum(4); }
		if (Input.GetKeyDown(KeyCode.Keypad5)) { setPoseNum(5); }
		if (Input.GetKeyDown(KeyCode.Keypad6)) { setPoseNum(6); }
		if (Input.GetKeyDown(KeyCode.Keypad7)) { setPoseNum(7); }
		if (Input.GetKeyDown(KeyCode.Keypad8)) { setPoseNum(8); }
		if (Input.GetKeyDown(KeyCode.Keypad9)) { setPoseNum(9); }

		//toggle whether mouse controls head
		if (Input.GetKeyDown(KeyCode.M)) { 
			ArduinoJointControl.mouseControl = !ArduinoJointControl.mouseControl; 
			mouseControl = ArduinoJointControl.mouseControl;
		}

		//control the face with key presses
		if (Input.GetKeyDown("0")) {face = 0;}
		if (Input.GetKeyDown("1")) {face = 1;}
		if (Input.GetKeyDown("2")) {face = 2;}
		if (Input.GetKeyDown("3")) {face = 3;}
		if (Input.GetKeyDown("4")) {face = 4;}
		if (Input.GetKeyDown("5")) {face = 5;}
		if (Input.GetKeyDown("6")) {face = 6;}
		if (Input.GetKeyDown("7")) {face = 7;}
		if (Input.GetKeyDown("8")) {face = 8;}
		if (Input.GetKeyDown("9")) {face = 9;}

		//clear the pose with backspace
		if (Input.GetKeyDown(KeyCode.Backspace)) { setPoseNum(-1); }
	}

	void setPoseNum(int num) {
		//set pose number
		poseNum = num; 	
		//set pose according to pose number
		if(0 <= poseNum && poseNum < 10 && poseNum < poses.Length) {
			pose = poses[poseNum].pose;
		} else {
			pose = null;
		}	
	}
}




