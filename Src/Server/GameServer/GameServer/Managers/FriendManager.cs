using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class FriendManager
    {
        private Character Owner;
        List<NFriendInfo> friends = new List<NFriendInfo>();
        private bool friendChanged = false;

        public FriendManager(Character owner)
        {
            this.Owner = owner;
            this.InitFriends();
        }

        public void GetFriendInfos(List<NFriendInfo> list)
        {
            foreach (var f in this.friends)
            {
                list.Add(f);
            }
        }
        private void InitFriends()
        {
           this.friends.Clear();
           foreach (var friend in this.Owner.Data.Friends)
           {
               this.friends.Add(GetFriendInfo(friend));
           }
        }
      
        public void AddFriend(Character friend)
        {
            TCharacterFriend tf = new TCharacterFriend()
            {
                FriendID = friend.Id,
                FriendName = friend.Data.Name,
                Class = friend.Data.Class,
                Level = friend.Data.Level
            };
            this.Owner.Data.Friends.Add(tf);
            friendChanged = true;
        }

        public bool RemoveFriendByFriendId(int friendId)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(v => v.FriendID == friendId);
            if (removeItem != null)
            {
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
            }

            friendChanged = true;
            return true;
        }

        public bool RemoveFriendByID(int id)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(v => v.Id == id);
            if (removeItem != null)
            {
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
            }

            friendChanged = true;
            return true;
        }
        public NFriendInfo GetFriendInfo(TCharacterFriend friend)
        {
           NFriendInfo friendInfo = new NFriendInfo();
           var character = CharacterManager.Instance.GetCharacter(friend.FriendID);
           friendInfo.frindInfo = new NCharacterInfo();
           friendInfo.Id = friend.Id;
           if (character == null)
           {
               friendInfo.frindInfo.Id = friend.FriendID;
               friendInfo.frindInfo.Name = friend.FriendName;
               friendInfo.frindInfo.Class = (CharacterClass) friend.Class;
               friendInfo.frindInfo.Level = friend.Level;
               friendInfo.Status = 0;
           }
           else
           {
               friendInfo.frindInfo = character.GetBasicInfo();
               friendInfo.frindInfo.Name = character.Info.Name;
               friendInfo.frindInfo.Class = character.Info.Class;
               friendInfo.frindInfo.Level = character.Info.Level;
               if (friend.Level != character.Info.Level)
               {
                   friend.Level = character.Info.Level;
               }
                character.FriendManager.UpdateFriendInfo(this.Owner.Info,1);
               friendInfo.Status = 1;
            }

           return friendInfo;
        }

        public NFriendInfo GetFriendInfo(int friendId)
        {
            foreach (var f in this.friends)
            {
                if (f.frindInfo.Id == friendId)
                {
                    return f;
                }
            }

            return null;
        }
        public void UpdateFriendInfo(NCharacterInfo friendInfo, int status)
        {
            foreach (var f in this.friends)
            {
                if (f.frindInfo.Id == friendInfo.Id)
                {
                    f.Status = status;
                    break;
                }
            }

            this.friendChanged = true;
        }

        public void OfflineNotify()
        {
            foreach (var friendInfo in this.friends)
            {
                var friend = CharacterManager.Instance.GetCharacter(friendInfo.frindInfo.Id);
                if (friend != null)
                {
                    friend.FriendManager.UpdateFriendInfo(this.Owner.Info,0);
                }
            }
        }

        public void PostProcess(NetMessageResponse message)
        {
            if (friendChanged)
            {
                this.InitFriends();
                if (message.friendList == null)
                {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(this.friends);
                }

                friendChanged = false;
            }
        }

    
    }
}
