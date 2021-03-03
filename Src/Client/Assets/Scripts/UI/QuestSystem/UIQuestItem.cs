using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem
{

    public Text title;

    public Image background;

    public Sprite normalbg;

    public Sprite selectedBg;

    public Quest quest;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalbg;
    }


    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetQuestInfo(Quest value)
    {
        this.quest = value;
        if (this.title != null)
            this.title.text = this.quest.Define.Name;
    }
}
