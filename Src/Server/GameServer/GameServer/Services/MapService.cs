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
    class MapService:Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
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
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(entitySync);
            byte[] data = PackageHandler.PackMessage(message);
            connection.SendData(data,0,data.Length);
        }
    }
}
