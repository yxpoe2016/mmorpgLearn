using System;
using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

public class GuildManager : Singleton<GuildManager> {
    public NGuildInfo guildInfo;
    internal NGuildMemberInfo myMemberInfo;

    public bool HasGuild
    {
        get { return this.guildInfo != null; }
    }

    // Use this for initialization
    public void Init(SkillBridge.Message.NGuildInfo guild)
    {
        this.guildInfo = guild;
        if (guild == null)
        {
            myMemberInfo = null;
            return;
        }

        foreach (var member in guild.Members)
        {
            if (member.characterId == User.Instance.CurrentCharacter.Id)
            {
                myMemberInfo = member;
                break;
            }
        }
    }

    public void ShowGuild()
    {
        if (this.HasGuild)
            UIManager.Instance.Show<UIGuild>();
        else
        {
            var win = UIManager.Instance.Show<UIGuildPopNoGuild>();
            win.OnClose += PopNoGuild_OnClose;
        }
    }

    private void PopNoGuild_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        if (result == UIWindow.WindowResult.Yes)
        {
            UIManager.Instance.Show<UIGuildPopCreate>();
        }
        else
        {
            UIManager.Instance.Show<UIGuildList>();
        }
    }
}
