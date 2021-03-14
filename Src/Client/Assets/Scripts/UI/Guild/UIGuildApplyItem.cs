using System;
using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildApplyItem : ListView.ListViewItem
{
    public Text nickname;

    public Text @class;

    public Text level;

    public NGuildApplyInfo Info;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void SetItemInfo(NGuildApplyInfo item)
    {
        this.Info = item;
        if (this.nickname != null) this.nickname.text = this.Info.Name;
        if (this.@class != null) this.@class.text = this.Info.Class.ToString();
        if (this.level != null) this.level.text = this.Info.Level.ToString();
    }

    public void OnAccept()
    {
        var confirm = MessageBox.Show(string.Format("{0}申请加入公会", this.Info.Name), "审批申请", MessageBoxType.Confirm, "同意", "拒绝");
        confirm.OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(true,this.Info);
        };
    }

    public void OnDecline()
    {
        var confirm = MessageBox.Show(string.Format("{0}申请加入公会", this.Info.Name), "审批申请", MessageBoxType.Confirm, "同意", "拒绝");
        confirm.OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(false, this.Info);
        };
    }

}
