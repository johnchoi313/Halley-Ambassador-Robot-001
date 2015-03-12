using UnityEngine;
using System.Collections;

public class AndroidOSC : MonoBehaviour {

	/* --- PRE SETUP --- */
	//osc vars
	public string RemoteIP = "127.0.0.1";
	public int outgoingPort = 12345;
	public int incomingPort = 1234;
	public static int outMessage;
	public static int inMessage;
	public UDPPacketIO udp;
	public Osc handler;
	
	/* ----- SETUP ----- */
	void Start () {
		// Set up OSC connection
		udp = (UDPPacketIO)GetComponent("UDPPacketIO");
		udp.init(RemoteIP, outgoingPort, incomingPort);
		handler = (Osc)GetComponent("Osc");
		handler.init(udp);
		
		// Listen to the channels set in the Processing sketch
		handler.SetAddressHandler("/data", ListenEvent);
	}
	//on receive data
	public void ListenEvent(OscMessage oscMessage) {
		// Make the data available 
		inMessage = (int) oscMessage.Values[0];
		Debug.Log(inMessage);
	}

	//show to output.
    void OnGUI() {
		GUI.Label(new Rect(0,0,200,100), "The inMessage is: "+inMessage.ToString());
	}

}
