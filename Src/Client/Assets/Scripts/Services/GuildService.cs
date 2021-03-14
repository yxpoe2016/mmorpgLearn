using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

public class GuildService : Singleton<GuildService>, IDisposable
{
    public UnityAction<bool> OnGuildCreateResult;
    public UnityAction OnGuildUpdate;
    public UnityAction<List<NGuildInfo>> OnGuildListResult;

    public void Init()
    {

    }
    public GuildService()
    {
        MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate);
        MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
        MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
        MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
        MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
        MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);
        MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdmin);
    }

    

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate);
        MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
        MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
        MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
        MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
        MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);
        MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(this.OnGuildAdmin);
    }

    public void SendGuildCreate(string guildName, string notice)
    {
        Debug.LogFormat("guildName:{0}", guildName);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.guildCreateReq = new GuildCreateRequest();
        message.Request.guildCreateReq.GuildName = guildName;
        message.Request.guildCreateReq.GuildNotice = notice;
        NetClient.Instance.SendMessage(message);
    }

    private void OnGuildCreate(object sender, GuildCreateResponse message)
    {
        if (OnGuildCreateResult!=null)
        {
            this.OnGuildCreateResult(message.Result == Result.Success);
        }

        if (message.Result == Result.Success)
        {
            GuildManager.Instance.Init(message.guildInfo);
            MessageBox.Show("创建公会成功");
        }
        else
            MessageBox.Show("创建公会失败");
    }

    public void SendGuildJoinRequest(int guildId)
    {
        Debug.LogFormat("guildId:{0}", guildId);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.guildJoinReq = new GuildJoinRequest();
        message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
        message.Request.guildJoinReq.Apply.GuildId = guildId;
        NetClient.Instance.SendMessage(message);
    }

    public void SendGuildJoinRespone(bool accept, GuildJoinRequest request)
    {
        Debug.Log("SendGuildJoinRespone");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.guildJoinRes = new GuildJoinResponse();
        message.Request.guildJoinRes.Apply = request.Apply;
        message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
        message.Request.guildJoinRes.Result = Result.Success;
        NetClient.Instance.SendMessage(message);
    }



    private void OnGuildJoinRequest(object sender, GuildJoinRequest message)
    {
        var confirm = MessageBox.Show(string.Format("{0}申请加入公会", message.Apply.Name), "公会申请", MessageBoxType.Confirm,"同意", "拒绝");
        confirm.OnYes = () =>
        {
            this.SendGuildJoinRespone(true, message);
        };
        confirm.OnNo = () =>
        {
            this.SendGuildJoinRespone(false, message);
        };
    }

    private void OnGuildJoinResponse(object sender, GuildJoinResponse message)
    {
        if (message.Result == Result.Success)
        {
            MessageBox.Show(message.Apply.Name+"加入公会成功", "公会");
        }
        else
            MessageBox.Show(message.Apply.Name + "加入公会失败", "公会");
    }

    private void OnGuild(object sender, GuildResponse message)
    {
        GuildManager.Instance.Init(message.guildInfo);
        if (this.OnGuildUpdate != null)
            this.OnGuildUpdate();
    }

    public void SendGuildLeaveRequest()
    {
        Debug.Log("SendGuildLeaveRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.guildLeave = new GuildLeaveRequest();
        NetClient.Instance.SendMessage(message);
    }
    private void OnGuildLeave(object sender, GuildLeaveResponse message)
    {
        if (message.Result == Result.Success)
        {
            GuildManager.Instance.Init(null);
            MessageBox.Show("离开公会成功", "公会");
        }
        else
            MessageBox.Show("离开公会失败", "公会",MessageBoxType.Error);
    }

    public void SendGuildListRequest()
    {
        Debug.Log("SendGuildListRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.guildList = new GuildListRequest();
        NetClient.Instance.SendMessage(message);
    }

    private void OnGuildList(object sender, GuildListResponse message)
    {
        if (OnGuildListResult != null)
            this.OnGuildListResult(message.Guilds);
    }

    public void SendGuildJoinApply(bool accpet, NGuildApplyInfo apply)
    {
        Debug.Log("SendGuildJoinApply");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.guildJoinRes = new GuildJoinResponse();
        message.Request.guildJoinRes.Apply = apply;
        message.Request.guildJoinRes.Apply.Result = accpet ? ApplyResult.Accept : ApplyResult.Reject;
        message.Request.guildJoinRes.Result = Result.Success;
        NetClient.Instance.SendMessage(message);
    }

    public void SendAdminCommand(GuildAdminCommand command, int characterId)
    {
        Debug.Log("SendAdminCommand");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.guildAdmin = new GuildAdminRequest();
        message.Request.guildAdmin.Command = command;
        message.Request.guildAdmin.Target = characterId;
        NetClient.Instance.SendMessage(message);
    }

    private void OnGuildAdmin(object sender, GuildAdminResponse message)
    {
        Debug.LogFormat("GuildAdmin : {0} {1}",message.Command,message.Result);
        MessageBox.Show(string.Format("执行操作：{0} 结果{1} {2}", message.Command, message.Result, message.Errormsg));
    }

}
