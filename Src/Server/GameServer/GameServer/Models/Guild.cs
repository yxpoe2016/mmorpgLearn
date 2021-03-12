using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Models
{
    class Guild
    {
        public int Id
        {
            get { return this.Data.Id; }
        }

        private Character leader;
        public string Name
        {
            get { return this.Data.Name; }
        }

        public List<Character> Members = new List<Character>();
        public double timestamp;
        public TGuild Data;

        public Guild(TGuild guild)
        {
            this.Data = guild;
        }

        public bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.GuildApplies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null)
                return false;

            var dbApply = DBService.Instance.Entities.TGuildApplies.Create();
            dbApply.TGuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.TGuildApplies.Add(dbApply);
            this.Data.GuildApplies.Add(dbApply);
            DBService.Instance.Save();
            this.timestamp = Time.timestamp;
            return true;
        }
        internal bool JoinAppove(NGuildApplyInfo apply)
        {
            var oldApply =
                this.Data.GuildApplies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if (oldApply == null)
                return false;

            oldApply.Result = (int) apply.Result;
            if (apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }
            DBService.Instance.Save();
            this.timestamp = Time.timestamp;
            return true;
        }

        public void AddMember(int characterId, string name, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharacterId = characterId,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now
            };
            this.Data.GuildMembers.Add(dbMember);
            timestamp = Time.timestamp;
        }
        internal void Leave(Character character)
        {
            this.Members.Remove(character);
            if (character == this.leader)
            {
                if (this.Members.Count > 0)
                    this.leader = this.Members[0];
                else
                    this.leader = null;
            }

            character.Guild = null;
            timestamp = Time.timestamp;
        }

        internal void PostProcess(Character from, NetMessageResponse message)
        {
            if (message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);
            }
        }

        internal NGuildInfo GuildInfo(Character character)
        {
            NGuildInfo info = new NGuildInfo()
            {
                Id = this.Id,
                GuildName = this.Name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderID,
                leaderName = this.Data.LeaderName,
                createTime = DateTime.Now.ToFileTime(),
                memberCount = this.Members.Count
            };
            if (character != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if(character.Id == this.Data.LeaderID)
                    info.Applies.AddRange(GetApplyInfos());
            }

            return info;
        }

        private List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();

            foreach (var member in this.Data.GuildMembers)
            {
                var memberInfo = new NGuildMemberInfo()
                {
                    Id = member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle) member.Title,
                    joinTime = member.JoinTime.ToFileTime(),
                    lastTime = member.LastTime.ToFileTime()
                };

                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                    if (member.Id == this.Data.LeaderID)
                        this.leader = character;
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                    if (member.Id == this.Data.LeaderID)
                        this.leader = null;
                }
                members.Add(memberInfo);
            }

            return members;
        }

        private NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id = member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level
            };
        }

        private List<NGuildApplyInfo> GetApplyInfos()
        {
          List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
          foreach (var apply in this.Data.GuildApplies)
          {
              applies.Add(new NGuildApplyInfo()
              {
                  characterId = apply.CharacterId,
                  GuildId = apply.TGuildId,
                  Class = apply.Class,
                  Level = apply.Level,
                  Name = apply.Name,
                  Result = (ApplyResult)apply.Result
              });
          }

          return applies;
        }
    }
}
