
using Common.Data;
using SkillBridge.Message;
using System;
using UnityEngine;

/// <summary>
/// 本地映射
/// </summary>
public class User : Singleton<User>
{
    private SkillBridge.Message.NUserInfo userInfo;
    public int CurrentRide = 0;

    public SkillBridge.Message.NUserInfo Info
    {
        get { return userInfo; }
    }

    public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
    {
        this.userInfo = info;
    }

    public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

    public MapDefine CurrentMapData { get; set; }

    public PlayerInputController CurrentCharacterObject { get; set; }

    public NTeamInfo TeamInfo { get; set; }

    internal void AddGold(int value)
    {
        this.CurrentCharacter.Gold += value;
    }

    public void Ride(int id)
    {
        if (CurrentRide != id)
        {
            CurrentRide = id;
            CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
        }
        else
        {
            CurrentRide = 0;
            CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
        }
    }
}
