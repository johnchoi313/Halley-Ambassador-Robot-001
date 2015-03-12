using UnityEngine;
using System.Collections;

public class setFaceRotation : MonoBehaviour {
	public Transform neckBone;
	public Transform facePos;
	void Update () { 
		Vector3 rot = neckBone.eulerAngles;
		transform.eulerAngles = new Vector3(40+rot.x,-rot.y,rot.z); 
		transform.position = facePos.position;
	}
}
