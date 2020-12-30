using System.Collections;
using System.Collections.Generic;
using Network;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Networking;

public class NetTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Debug.Log(NetClient.Instance);
		NetClient.Instance.Init("127.0.0.1",8000);
		NetClient.Instance.Connect();

		NetMessage msg = new NetMessage();
        msg.Request = new NetMessageRequest();
		msg.Request.firstRequest = new FirstTestRequest();
		msg.Request.firstRequest.Helloworld = "Hello World!";
		NetClient.Instance.SendMessage(msg);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
