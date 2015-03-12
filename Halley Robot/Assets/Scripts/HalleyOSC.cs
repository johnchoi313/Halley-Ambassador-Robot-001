using UnityEngine;
using System.Collections;

public class HalleyOSC : MonoBehaviour {
	
	/* --- PRE SETUP --- */
	//osc vars
	public string RemoteIP = "127.0.0.1";
	public int outgoingPort = 12345;
	public int incomingPort = 1234;
	public static int outMessage = 0;
	public static int inMessage = 0;
	public UDPPacketIO udp;
	public Osc handler;

	//time variables
	public float delay = 0;
    public float wait;

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
	/* --- MAIN LOOP --- */
	void Update () {

		//set the outMessage as the current face
		outMessage = PoseControl.face;

        //send data
		if(delay <= 0) {
			handler.Send(Osc.StringToOscMessage("/data "+outMessage.ToString()));
			delay = wait;
		} else {
			delay -= Time.deltaTime;
		}

	}

}
