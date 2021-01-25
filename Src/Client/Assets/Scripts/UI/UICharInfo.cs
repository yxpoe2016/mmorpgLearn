using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour
{

    public NCharacterInfo info;

    public Text charClass;

    public Text charName;

    public Image SelectImage;

    public bool IsSelect
    {
        get { return SelectImage.IsActive(); }
        set { SelectImage.gameObject.SetActive(value);}
    }

	// Use this for initialization
	void Start () {
        if (info!=null)
        {
            this.charClass.text = this.info.Class.ToString();
            this.charName.text = this.info.Name;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
