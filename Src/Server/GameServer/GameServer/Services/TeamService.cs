using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class TeamService:Singleton<TeamService>
    {
        public void Init()
        {
            TeamManager.Instance.Init();
        }

        public TeamService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.OnTeamLeave);
        }

        private void OnTeamInviteRequest(NetConnection<NetSession> sender, TeamInviteRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteRequest:FromId{0} FromName{1} ToID{2} ToName{3}TeamID{4}", message.FromId, message.FromName, message.ToId, message.ToName,message.TeamId);
            

            NetConnection<NetSession> target = SessionManager.Instance.GetSession(message.ToId);
       

            if (target == null)
            {
                sender.Session.Response.teamInviteRes= new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Errormsg = "好友不存在或者不在线";
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.SendResponse();
                return;
            }

            if (target.Session.Character.Team != null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Errormsg = "对方已经有队伍";
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwarTeamInviteRequest:FromId{0} FromName{1} ToID{2} ToName", message.FromId, message.FromName, message.ToId, message.ToName);
            target.Session.Response.teamInviteReq = message;
            target.SendResponse();
        }

        private void OnTeamInviteResponse(NetConnection<NetSession> sender, TeamInviteResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteResponse:character{0} Result{1} ToID{2}", character.Id, message.Result, message.Request.ToId);
            sender.Session.Response.teamInviteRes = message;
            if (message.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.teamInviteRes.Result = Result.Failed;
                    sender.Session.Response.teamInviteRes.Errormsg = "请求者已下线";
                }
                else
                {
                    TeamManager.Instance.AddTeamMember(requester.Session.Character, character);//队长，成员
                   int teamid =  message.Request.TeamId;
                    requester.Session.Response.teamInviteRes = message;
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        private void OnTeamLeave(NetConnection<NetSession> sender, TeamLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse:character{0} teamID{1} ", character.Id, message.TeamId);
            sender.Session.Response.teamLeave = new TeamLeaveResponse();
            sender.Session.Response.teamLeave.Result = Result.Success;
            sender.Session.Response.teamLeave.characterId = message.characterId;

            for (int i = 0; i < TeamManager.Instance.Teams.Count; i++)
            {
                if (TeamManager.Instance.Teams[i].Id == message.TeamId)
                {
                    TeamManager.Instance.Teams[i].Leave(character);
                }
            }
            sender.SendResponse();
        }

    }
}
