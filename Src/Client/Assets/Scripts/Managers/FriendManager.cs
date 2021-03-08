using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

public class FriendManager : Singleton<FriendManager>
{

    public List<NFriendInfo> allFriends = new List<NFriendInfo>();

    public void Init(List<NFriendInfo> friends)
    {
        this.allFriends = friends;
    }
}
