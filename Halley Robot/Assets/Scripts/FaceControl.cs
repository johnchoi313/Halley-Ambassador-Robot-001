using UnityEngine;
using System.Collections;

public class FaceControl : MonoBehaviour {

	/* --- PRE SETUP --- */
	public bool android = false;
	public Texture[] faces;
	private int face = 0;
	private int oldFace = 1;
	/***Face Info***
	 * 0 = Standard
	 * 1 = Blink
	 * 2 = Big
	 * 3 = Small
	 * 4 = Angry 
	 * 5 = Laugh
	 * 6 = Sad */
    
	/* ----- SETUP ----- */
	void Start () {
		updateFaceTexture();
	}

	/* --- MAIN LOOP --- */
	void Update () {
		//if we are on Android:
		if(android == true) {
			//get the inMessage as the face index.
			face = AndroidOSC.inMessage;
		} else {
			//get the face pose as the face index.
			face = PoseControl.face;
		}
		//update the face texture.
		updateFaceTexture();
	}
	void updateFaceTexture() {
		//check if there is at least one texture in faces
		if(faces.Length > 0) {
			//clamp the face index
			if(face < 0) {face = 0;}
			else if(face >= faces.Length){face = faces.Length -1;}
			//switch the face texture if it changed
			if(face != oldFace) {
				renderer.material.SetTexture("_MainTex",faces[face]);
				oldFace = face;
			}
		}
	}
}


