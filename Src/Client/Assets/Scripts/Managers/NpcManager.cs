using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Common.Data;
using UnityEngine;

public class NpcManager : Singleton<NpcManager>
{

    public delegate bool NpcActionHandler(NpcDefine npc);

    Dictionary<NpcFunction,NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();
    Dictionary<int,Vector3> npcPosition = new Dictionary<int, Vector3>();
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

    private bool Interactive(NpcDefine npc)
    {
        if (DoTaskInteractive(npc))
        {
            return true;
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
        var status = QuestManager.Instance.GetQuestStatusByNpc(npc.ID);
    
        if (status == NpcQuestStatus.None)
            return false;

        return QuestManager.Instance.OpenNpcQuest(npc.ID);
    }

    public void UpdateNpcPosition(int npc, Vector3 pos)
    {
        this.npcPosition[npc] = pos;
    }
    internal Vector3 GetNpcPosition(int npc)
    {
        return this.npcPosition[npc];
    }

    public NpcDefine GetNpcDefine(int id)
    {
        NpcDefine npc = null;
        DataManager.Instance.NPCs.TryGetValue(id, out npc);
        return npc;
    }
}
