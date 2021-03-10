using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class MapService:Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }

        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapEntitySync :charaterID {0}:{1} SPEED{2}",character.Id,character.Info.Name,character.EntityData.Speed);
            MapManager.Instance[character.Info.mapId].UpdateEntity(message.entitySync);
        }

        internal void SendEntityUpdate(NetConnection<NetSession> connection, NEntitySync entitySync)
        {
            connection.Session.Response.mapEntitySync = new MapEntitySyncResponse();
            connection.Session.Response.mapEntitySync.entitySyncs.Add(entitySync);
            connection.SendResponse();
            // NetMessage message = new NetMessage();
            // message.Response = new NetMessageResponse();
            // message.Response.mapEntitySync = new MapEntitySyncResponse();
            // message.Response.mapEntitySync.entitySyncs.Add(entitySync);
            // byte[] data = PackageHandler.PackMessage(message);
            // connection.SendData(data,0,data.Length);
        }
        private void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest message)
        {
            Character character = sender.Session.Character;
            if (!DataManager.Instance.Teleporters.ContainsKey(message.teleporterId))
            {
                return;
            }

            TeleporterDefine source = DataManager.Instance.Teleporters[message.teleporterId];
            if (source.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(source.LinkTo))
            {
                Log.WarningFormat("Source TeleportID {0}" ,message.teleporterId);
            }

            TeleporterDefine target = DataManager.Instance.Teleporters[source.LinkTo];
            MapManager.Instance[source.MapID].CharacterLeave(character);
            character.Position = target.Position;
            character.Direction = target.Direction;
            MapManager.Instance[target.MapID].CharacterEnter(sender,character);

        }
    }
}
