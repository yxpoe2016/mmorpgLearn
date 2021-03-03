using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour
{

    public Transform owner;

    public float heght = 2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (owner != null)
        {
            this.transform.position = owner.transform.position + Vector3.up * heght;
        }
		if(Camera.main!=null)
            transform.forward = Camera.main.transform.forward;
	}
}
