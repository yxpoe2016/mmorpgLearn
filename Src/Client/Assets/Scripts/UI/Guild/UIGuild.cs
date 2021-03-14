﻿using System;
using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

public class UIGuild : UIWindow
{

    public GameObject itemPrefab;

    public ListView listMain;

    public Transform itemRoot;

    public UIGuildInfo uiInfo;

    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;

    public GameObject panelLeader;
	// Use this for initialization
	void Start ()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }

    void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    private void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;

        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
       this.listMain.RemoveAll();
    }

    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }

    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void OnClickLeave()
    {
        MessageBox.Show("扩张");
    }

    public void OnClickChat()
    {

    }

    public void OnClickKickout()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("确定要踢{0}队伍吗?",this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes =
            () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout,this.selectedItem.Info.Info.Id); };
    }

    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }

        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已经身份尊贵");
            return;
        }
        MessageBox.Show(string.Format("确定晋升{0}为副会长吗?", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm, "确定", "取消").OnYes =
            () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id); };

    }

    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }

        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方是个平民");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("对方是会长哦");
            return;
        }
        MessageBox.Show(string.Format("确定罢免{0} 吗?", this.selectedItem.Info.Info.Name), "罢免", MessageBoxType.Confirm, "确定", "取消").OnYes =
            () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id); };
    }

    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要转让的成员");
            return;
        }
        MessageBox.Show(string.Format("确认要把会长转让给{0} 吗?", this.selectedItem.Info.Info.Name), "会长转让", MessageBoxType.Confirm, "确定", "取消").OnYes =
            () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id); };
    }
    public void OnClickSetNotice()
    {
        MessageBox.Show("扩张");
    }
   
}
