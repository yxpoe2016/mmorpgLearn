using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildInfo : MonoBehaviour {

    public Text guildName;

    public Text guildID;

    public Text leader;

    public Text notice;

    public Text memberNumber;

    private NGuildInfo info;

    public NGuildInfo Info
    {
        get { return info; }
        set
        {
            this.info = value;
            this.UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (this.info == null)
        {
            this.guildName.text = "无";
            this.guildID.text = "无";
            this.leader.text = "无";
            this.notice.text = "无";
            this.memberNumber.text = "无";
        }
        else
        {
            this.guildName.text = this.info.GuildName;
            this.guildID.text = "ID: " + this.info.Id;
            this.leader.text = this.info.leaderName;
            this.notice.text = this.info.Notice;
            this.memberNumber.text =
                this.info.memberCount.ToString(); //string.Format("成员数量{1} / {2}", this.info.memberCount.ToString(), GameDefine.GuildMaxMemberCount.ToString());
        }
    }
}
