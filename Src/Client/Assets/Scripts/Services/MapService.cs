using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Managers;
using Common.Data;
using Network;
using Services;
using SkillBridge.Message;
using UnityEngine;

public class MapService : Singleton<MapService>, IDisposable
{
    public int CurrentMapId;

    public MapService()
    {
        MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
        MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
    }



    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
        MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
    }

    public void Init()
    {

    }

    private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
    {
        Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);
        foreach (var cha in response.Characters)
        {
            if (User.Instance.CurrentCharacter == null||User.Instance.CurrentCharacter.Id == cha.Id)
            {
                User.Instance.CurrentCharacter = cha;
            }

            CharacterManager.Instance.AddCharacter(cha);
            if (CurrentMapId != response.mapId)
            {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;
            }
        }
    }

    private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse message)
    {
        Debug.LogFormat("OnMapCharacterLeave?: CharID:{0}", message.characterId);
        if (message.characterId != User.Instance.CurrentCharacter.Id)
            CharacterManager.Instance.RemoveCharacter(message.characterId);
        else
            CharacterManager.Instance.Clear();
    }

    private void EnterMap(int mapId)
    {
        if (DataManager.Instance.Maps.ContainsKey(mapId))
        {
            MapDefine map = DataManager.Instance.Maps[mapId];
            User.Instance.CurrentMapData = map;
            SceneManager.Instance.LoadScene(map.Resource);
        }
        else
        {
            Debug.LogErrorFormat("EnterMap: Map{0} not existed", mapId);
        }
    }

    public void SendMapEntitySync(EntityEvent entityEvent,NEntity entity)
    {
        Debug.LogFormat("MapEntityRequest :ID:{0} POS:{1} DIR:{2} SPEED:{3} entityEvent {4}", entity.Id, entity.Position.String(), entity.Direction.String(), entity.Speed, entityEvent);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.mapEntitySync = new MapEntitySyncRequest();
        message.Request.mapEntitySync.entitySync = new NEntitySync()
        {
            Id = entity.Id,
            Event = entityEvent,
            Entity = entity
        };
       
        NetClient.Instance.SendMessage(message);

    }

    private void OnMapEntitySync(object sender, MapEntitySyncResponse message)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("MapEntityUpdateResponse: Entity : {0}", message.entitySyncs.Count);
        sb.AppendLine();

        foreach (var entity in message.entitySyncs)
        {
            EntityManager.Instance.OnEntitySync(entity);
            sb.AppendFormat("{0}evt:{1} entity:{2} SPEED{3}", entity.Id,entity.Event,entity.Entity.String(), entity.Entity.Speed);
            sb.AppendLine();
        }
    }

 
}

