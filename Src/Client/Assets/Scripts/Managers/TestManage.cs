using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;

public class TestManage : Singleton<TestManage> {

    public void Init()
    {
		NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop,OnNpcInvokeShop);
        NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
    }

    private bool OnNpcInvokeInsrance(NpcDefine npc)
    {
        MessageBox.Show("点击了NPC" + npc.Name, "NPC对话");
        return true;
    }

    private bool OnNpcInvokeShop(NpcDefine npc)
    {
       UITest test = UIManager.Instance.Show<UITest>();
        return true;
    }

 
}
