using System.Collections;
using System.Collections.Generic;
using Entities;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UINameBar : MonoBehaviour
{
    public Image avatar;
    public Text avaverName;

    public Character character;
	// Use this for initialization
	void Start () {
        if (character != null)
        {
            if(character.Info.Type==CharacterType.Monster)
                this.avatar.gameObject.SetActive(false);
            else
                this.avatar.gameObject.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.UpdateInfo();
        
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
