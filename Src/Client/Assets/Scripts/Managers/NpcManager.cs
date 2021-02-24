using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;

public class NpcManager : Singleton<NpcManager>
{

    public delegate bool NpcActionHandler(NpcDefine npc);

    Dictionary<NpcFunction,NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();

    public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
    {
        if (!eventMap.ContainsKey(function))
        {
            eventMap[function] = action;
        }
        else
        {
            eventMap[function] += action;
        }
    }

    public bool Interactive(int npcId)
    {
        if (DataManager.Instance.NPCs.ContainsKey(npcId))
        {
            var npc = DataManager.Instance.NPCs[npcId];
            return Interactive(npc);
        }

        return false;
    }

    public bool Interactive(NpcDefine npc)
    {
        if (npc.Type == NpcType.Task)
        {
            return DoTaskInteractive(npc);
        }
        else if (npc.Type == NpcType.Functional)
        {
            return DoFunctionInteractive(npc);
        }

        return false;
    }

    private bool DoFunctionInteractive(NpcDefine npc)
    {
        if (npc.Type!=NpcType.Functional)
            return false;

        if (!eventMap.ContainsKey(npc.Function))
            return false;
        return eventMap[npc.Function](npc);
    }

    private bool DoTaskInteractive(NpcDefine npc)
    {
        MessageBox.Show("点击了NPC" + npc.Name, "NPC对话");
        return true;
    }

    public NpcDefine GetNpcDefine(int id)
    {
        NpcDefine npc = null;
        DataManager.Instance.NPCs.TryGetValue(id, out npc);
        return npc;
    }
}
