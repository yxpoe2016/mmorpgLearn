﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;

namespace GameServer.Models
{
    class Chat
    {
        private Character Owner;

        public int localIdx;
        public int worldIdx;
        public int systemIdx;
        public int teamIdx;
        public int guildIdx;

        public Chat(Character owner)
        {
            this.Owner = owner;
        }

        public void PostProcess(NetMessageResponse message)
        {
            if (message.Chat == null)
            {
                message.Chat = new ChatResponse();
                message.Chat.Result = Result.Success;
            }

            this.localIdx = ChatManager.Instance.GetLocalMessages(this.Owner.Info.mapId, this.localIdx, message.Chat.localMessages);
            this.worldIdx = ChatManager.Instance.GetWorldMessages( this.worldIdx, message.Chat.localMessages);
            this.systemIdx = ChatManager.Instance.GetSystemMessages( this.systemIdx, message.Chat.localMessages);
            if (this.Owner.Team != null)
            {
                this.teamIdx = ChatManager.Instance.GetLocalMessages(this.Owner.Team.Id, this.teamIdx, message.Chat.localMessages);
            }
            if (this.Owner.Guild != null)
            {
                this.guildIdx = ChatManager.Instance.GetLocalMessages(this.Owner.Guild.Id, this.guildIdx, message.Chat.localMessages);
            }
        }
    }
}
