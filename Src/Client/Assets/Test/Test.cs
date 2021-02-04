using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        float h = Input.GetAxis("Horizontal");
        if (h < -0.1 || h > 0.1)
        {
            this.transform.Rotate(0, h * 2, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(new Vector3Int(1,0,0));
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);

            if (rot.eulerAngles.y > 100 && rot.eulerAngles.y < (360 - 100))
            {
              Debug.Log(dir);
            }

        }
    }
}
