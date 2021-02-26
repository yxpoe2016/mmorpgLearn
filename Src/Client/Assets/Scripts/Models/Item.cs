﻿using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

public class Item {

    public int Id;
    public int Count;
    public ItemDefine Define;

    public Item(NItemInfo item)
    {
        this.Id = item.Id;
        this.Count = item.Count;
        this.Define = DataManager.Instance.Items[item.Id];
    }

    public override string ToString()
    {
        return string.Format("ID:{0},Count:{1}", this.Id, this.Count);
    }
}
