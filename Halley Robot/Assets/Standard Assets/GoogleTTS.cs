using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

// By Jefferson Reis
// Note: Works only on Android, or platform that supports mp3 files.

public class GoogleTTS : MonoBehaviour {
	/* --- PRE SETUP --- */
	public string words = "Hello";
	public int sw = 0;
	public int sh = 0;

	/* --- TTS via Google Translate --- */
	IEnumerator Start () {
		//Remove the "spaces" in excess
		Regex rgx = new Regex ("\\s+");
		//Replace the "spaces" with "% 20" for the link can be interpreted
		string result = rgx.Replace (words, "%20");
		string url = "http://translate.google.com/translate_tts?tl=en&q=" + result;
		WWW www = new WWW (url);
		yield return www;
		//get the processed sound from Google Translate
		audio.clip = www.GetAudioClip (false, false, AudioType.MPEG);
		audio.Play ();
		words = "";
	}

	/* --- Draw GUI --- */
	void OnGUI () {

		//set word wrap to true.
		GUI.skin.textField.wordWrap = true;

		//update screen width and height
		sw = Screen.width; sh = Screen.height;

		//update Text Field input
		words = GUI.TextField(new Rect(10, 10, 300, 100), words);
		//send to Google Translate upon pressing Speak
		if (GUI.Button (new Rect(10, 120, 60, 30), "Speak")) {
		    StartCoroutine(Start());
		}

	}
}
