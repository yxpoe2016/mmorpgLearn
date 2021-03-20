using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class GuildManager : Singleton<GuildManager>
    {
        public Dictionary<int, Guild> Guilds = new Dictionary<int, Guild>();
        private HashSet<string> GuildNames = new HashSet<string>();

        public void Init()
        {
            this.Guilds.Clear();
            foreach (var guild in DBService.Instance.Entities.TGuilds)
            {
                this.AddGuild(new Guild(guild));
            }
        }

        private void AddGuild(Guild guild)
        {
            this.Guilds.Add(guild.Id, guild);
            this.GuildNames.Add(guild.Name);
            guild.timestamp = TimeUtil.timestamp;
        }

        internal bool CheackNameExisted(string guildName)
        {
            return GuildNames.Contains(guildName);
        }

        internal void CreateGuild(string guildName, string guildNotice, Character character)
        {
            DateTime now = DateTime.Now;
            TGuild dbGuild = DBService.Instance.Entities.TGuilds.Create();
            dbGuild.Name = guildName;
            dbGuild.Notice = guildNotice;
            dbGuild.LeaderID = character.Id;
            dbGuild.LeaderName = character.Name;
            dbGuild.CreateTime = now;
            DBService.Instance.Entities.TGuilds.Add(dbGuild);

            Guild guild = new Guild(dbGuild);
            guild.AddMember(character.Id, character.Name, character.Data.Class, character.Data.Level,
                GuildTitle.President);
            character.Guild = guild;
            DBService.Instance.Save();
            character.Data.GuildId = dbGuild.Id;
            DBService.Instance.Save();
            this.AddGuild(guild);
        }

        internal Guild GetGuild(int guildId)
        {
            if (guildId == 0)
                return null;
            Guild guild = null;
            this.Guilds.TryGetValue(guildId, out guild);
            return guild;
        }

        internal List<NGuildInfo> GetGuildsInfo()
        {
            List<NGuildInfo> result = new List<NGuildInfo>();
            foreach (var kv in this.Guilds)
            {
                result.Add(kv.Value.GuildInfo(null));
            }

            return result;
        }
    }
}
