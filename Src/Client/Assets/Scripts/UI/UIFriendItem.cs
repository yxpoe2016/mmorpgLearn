using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{
    public Text nickName;

    public Text @class;

    public Text level;

    public Text status;

    public Image background;

    public Sprite normalBg;

    public Sprite selectedBg;

    public NFriendInfo Info;
    private bool isEquiped = false;
    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetFriendInfo(NFriendInfo item)
    {
        this.Info = item;
        if (this.nickName != null) this.nickName.text = this.Info.frindInfo.Name;
        if (this.@class != null) this.@class.text = this.Info.frindInfo.Class.ToString();
        if (this.level != null) this.level.text = this.Info.frindInfo.Level.ToString();
        if (this.status != null) this.status.text = this.Info.Status == 1?"在线":"离线";
    }
}
