using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using SkillBridge.Message;
using UnityEngine;

public class QuestService : Singleton<QuestService>, IDisposable
{
    public QuestService()
    {
        MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnQuestAccept);
        MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnQuestSubmit);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnQuestAccept);
        MessageDistributer.Instance.Unsubscribe<ItemEquipResponse>(this.OnQuestSubmit);
    }


    private void OnQuestSubmit(object sender, ItemEquipResponse message)
    {
        
    }

    private void OnQuestAccept(object sender, ItemBuyResponse message)
    {
       
    }
}
