using Common.Data;
using GameServer.Core;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Managers;
using GameServer.Models;
using Network;

namespace GameServer.Entities
{
    class Character : CharacterBase,IPostResponser
    {
        public TCharacter Data;

        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public QuestManager QuestManager;
        public FriendManager FriendManager;
        public Guild Guild;
        public Team Team;
        public double TeamUpdateTS;
        public double GuildUpdateTS;
        public Chat Chat;
        public long Glod
        {
            get
            {
                return this.Data.Gold;
            }
            set
            {
                if (this.Data.Gold == value)
                    return;
                this.StatusManager.AddGoldChange((int) (value - this.Data.Gold));
                this.Info.Gold = value;
            }
        }

        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Info = new NCharacterInfo();
            this.Id = cha.ID;
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//cha.Level;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            this.ItemManager = new ItemManager(this);
            this.ItemManager.GetItemInfos(this.Info.Items);

            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            this.Info.Bag.Items = this.Data.Bag.Items;
            this.Info.Equips = this.Data.Equips;
            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);

            this.StatusManager = new StatusManager(this);
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);
            this.Guild = GuildManager.Instance.GetGuild(this.Data.GuildId);
            this.Chat = new Chat(this);
        }

        public void PostProcess(NetMessageResponse message)
        {
            Log.InfoFormat("PostProcess > Character: characterID{0}:{1}", this.Id, this.Info.Name);
            this.FriendManager.PostProcess(message);

            if (this.Team != null)
            {
                Log.InfoFormat("PostProcess > Character: characterID{0}:{1}  {2}<{3}", this.Id, this.Info.Name, TeamUpdateTS, this.Team.timestamp);
                if (TeamUpdateTS < this.Team.timestamp)
                {
                    TeamUpdateTS = this.Team.timestamp;
                    this.Team.PostProcess(message);
                }
            }

            if (this.Guild != null)
            {
                if (this.Info.Guild == null)
                {
                    this.Info.Guild = this.Guild.GuildInfo(this);
                    if (message.mapCharacterEnter != null)
                        GuildUpdateTS = this.Guild.timestamp;
                }

                if (GuildUpdateTS < this.Guild.timestamp && message.mapCharacterEnter == null)
                {
                    GuildUpdateTS = this.Guild.timestamp;
                    this.Guild.PostProcess(this,message);
                }
            }

            if (this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(message);
            }

            this.Chat.PostProcess(message);
        }

        public NCharacterInfo GetBasicInfo()
        {
            return new NCharacterInfo()
            {
                Id = this.Id,
                Name = this.Info.Name,
                Class = this.Info.Class,
                Level = this.Info.Level
            };
        }

        public void Clear()
        {
            this.FriendManager.OfflineNotify();
        }
    }
}
