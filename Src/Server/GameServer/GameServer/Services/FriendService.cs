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
    class FriendService:Singleton<FriendService>
    {

        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemove);
        }

        public void Init()
        {

        }
        private void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest:FromId{0} FromName{1} ToID{2} ToName{3}",message.FromId,message.FromName,message.ToId,message.ToName);
            if (message.ToId == 0)
            {
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if (cha.Value.Data.Name == message.ToName)
                    {
                        message.ToId = cha.Key;
                        break;
                    }
                }
            }

            NetConnection<NetSession> friend = null;
            if (message.ToId > 0)
            {
                if (character.FriendManager.GetFriendInfo(message.ToId) != null)
                {
                    sender.Session.Response.friendAddRes = new FriendAddResponse();
                    sender.Session.Response.friendAddRes.Errormsg = "已经是好友了";
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.SendResponse();
                    return;
                }

                friend = SessionManager.Instance.GetSession(message.ToId);
            }

            if (friend == null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或者不在线";
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.SendResponse();
                return;
            }
            Log.InfoFormat("ForwarRequest:FromId{0} FromName{1} ToID{2} ToName", message.FromId, message.FromName, message.ToId, message.ToName);
            friend.Session.Response.friendAddReq = message;
            friend.SendResponse();
        }
        private void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse:character{0} Result{1} ToID{2}", character.Id, message.Result, message.Request.ToId);
            sender.Session.Response.friendAddRes = message;
            if (message.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已下线";
                }
                else
                {
                    character.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = message;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "添加好友成功";
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        private void OnFriendRemove(NetConnection<NetSession> sender, FriendRemoveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse:character{0} FriendReletionID{1} ", character.Id, message.Id);
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = message.Id;
            if (character.FriendManager.RemoveFriendByID(message.Id))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;
                var friend = SessionManager.Instance.GetSession(message.friendId);
                if (friend != null)
                {
                    friend.Session.Character.FriendManager.RemoveFriendByFriendId(character.Id);
                }
                else
                {
                    this.RemoveFriend(message.friendId, character.Id);
                }
            }
            else
                sender.Session.Response.friendRemove.Result = Result.Failed;

            DBService.Instance.Save();
        }

        private void RemoveFriend(int friendId, int charId)
        {
            var removeItem =
                DBService.Instance.Entities.TCharacterFriends.FirstOrDefault(v =>
                    v.CharacterID == charId && v.FriendID == friendId);
            if(removeItem!=null)
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
        }
    }
}
