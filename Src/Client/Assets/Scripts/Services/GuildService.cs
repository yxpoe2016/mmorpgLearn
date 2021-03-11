using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using SkillBridge.Message;
using UnityEngine;

public class GuildService : Singleton<GuildService>, IDisposable
{
    public GuildService()
    {
        MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate);
        MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
        MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
        MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
        MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
        MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);
    }
    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate);
        MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
        MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
        MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
        MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
        MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);
    }

    private void OnGuild(object sender, GuildResponse message)
    {
    }

    private void OnGuildList(object sender, GuildListResponse message)
    {
    }

    private void OnGuildLeave(object sender, GuildLeaveResponse message)
    {
    }

    private void OnGuildJoinResponse(object sender, GuildJoinResponse message)
    {
    }

    private void OnGuildJoinRequest(object sender, GuildJoinRequest message)
    {
    }

    private void OnGuildCreate(object sender, GuildCreateResponse message)
    {
    }


    public void Init()
    {

    }
}
