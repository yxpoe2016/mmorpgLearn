using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour
{
    public Text avaverName;

    public Character character;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.UpdateInfo();
        transform.forward = Camera.main.transform.forward;
    }

    void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = "Lv." + character.Info.Level + character.Name;
            if (this.avaverName.text != name)
            {
                this.avaverName.text = name;
            }
        }
	}
}
