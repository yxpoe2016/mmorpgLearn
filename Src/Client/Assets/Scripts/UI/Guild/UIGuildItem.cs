using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem {
    public NGuildInfo Info { get; internal set; }

    public Text id;

    public Text Name;

    public Text Num;

    public Text Leader;

    public Image background;

    public Sprite normalBg;

    public Sprite selectedBg;

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

    internal void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.id != null) this.id.text = this.Info.Id.ToString();
        if (this.Name != null) this.Name.text = this.Info.GuildName.ToString();
        if (this.Num != null) this.Num.text = this.Info.memberCount.ToString();
        if (this.Leader != null) this.Leader.text = this.Info.leaderName;
    }
}
