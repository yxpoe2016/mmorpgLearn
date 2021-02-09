﻿
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

/// <summary>
/// 本地映射
/// </summary>
public class User : Singleton<User>
{
    private SkillBridge.Message.NUserInfo userInfo;


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

        public GameObject CurrentCharacterObject { get; set; }

}
