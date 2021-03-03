using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using SkillBridge.Message;
using UnityEngine;

public enum NpcQuestStatus
{
    None = 0,
    Complete,//拥有已完成可提交任务
    Available,//拥有可接受任务
    Incomplete,//拥有未完成任务
}
public class QuestManager : Singleton<QuestManager>
{
    public List<NQuestInfo> questInfos;
    public Dictionary<int,Quest> allQuests = new Dictionary<int, Quest>();
    public Dictionary<int,Dictionary<NpcQuestStatus,List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();


    public void Init(List<NQuestInfo> quests)
    {
        this.questInfos = quests;
        allQuests.Clear();
        this.npcQuests.Clear();
        InitQuests();
    }

    private void InitQuests()
    {
        foreach (var info in this.questInfos)
        {
            Quest quest = new Quest(info);
            this.AddNpcQuest(quest.Define.AcceptNPC, quest);
            this.AddNpcQuest(quest.Define.SubmitNPC, quest);
            this.allQuests[quest.Info.QuestId] = quest;
        }

        foreach (var kv in DataManager.Instance.Quests)
        {
            if (kv.Value.LimitClass != CharacterClass.None &&
                kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
            continue;
            if(kv.Value.LimitLevel>User.Instance.CurrentCharacter.Level)
                continue;
            if(this.allQuests.ContainsKey(kv.Key))
                continue;
            if (kv.Value.PreQuest > 0)
            {
                Quest preQuest;
                if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))
                {
                    if(preQuest.Info==null)
                        continue;
                    if(preQuest.Info.Status!=QuestStatus.Failed)
                        continue;
                }
                else
                    continue;
            }
            Quest quest = new Quest(kv.Value);
            this.AddNpcQuest(quest.Define.AcceptNPC, quest);
            this.AddNpcQuest(quest.Define.SubmitNPC, quest);
            this.allQuests[quest.Define.ID] = quest;
        }
    }

    private void AddNpcQuest(int npcId, Quest quest)
    {
        if (!this.npcQuests.ContainsKey(npcId))
        {
            this.npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
        }

        List<Quest> availables;
        List<Quest> complates;
        List<Quest> incomplates;
        if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
        {
            availables = new List<Quest>();
            this.npcQuests[npcId][NpcQuestStatus.Available] = availables;
        }
        if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete, out complates))
        {
            complates = new List<Quest>();
            this.npcQuests[npcId][NpcQuestStatus.Complete] = complates;
        }
        if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incomplates))
        {
            incomplates = new List<Quest>();
            this.npcQuests[npcId][NpcQuestStatus.Incomplete] = incomplates;
        }

        if (quest.Info == null)
        {
            if (npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
            {
                this.npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
            }
        }
        else
        {
            if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Complated)
            {
                if (!this.npcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                {
                    this.npcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                }
            }
            if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
            {
                if (!this.npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                {
                    this.npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                }
            }
        }
    }

    public NpcQuestStatus GetQuestStatusByNpc(int npcId)
    {
        Dictionary<NpcQuestStatus,List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
        if (this.npcQuests.TryGetValue(npcId, out status))
        {
            if (status[NpcQuestStatus.Complete].Count > 0)
                return NpcQuestStatus.Complete;
            if (status[NpcQuestStatus.Available].Count > 0)
                return NpcQuestStatus.Available;
            if (status[NpcQuestStatus.Incomplete].Count > 0)
                return NpcQuestStatus.Incomplete;
        }

        return NpcQuestStatus.None;
    }

    public bool OpenNpcQuest(int npcId)
    {
        Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
        if (this.npcQuests.TryGetValue(npcId, out status))
        {
            if (status[NpcQuestStatus.Complete].Count > 0)
                return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
            if (status[NpcQuestStatus.Available].Count > 0)
                return ShowQuestDialog(status[NpcQuestStatus.Available].First());
            if (status[NpcQuestStatus.Incomplete].Count > 0)
                return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
        }

        return false;
    }

    private bool ShowQuestDialog(Quest quest)
    {
        if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
        {
            UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
            dlg.SetQuest(quest);
            dlg.OnClose += OnQuestDialogClose;
            return true;
        }

        if (quest.Info != null || quest.Info.Status == QuestStatus.Complated)
        {
            if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                MessageBox.Show(quest.Define.DialogIncomplete);
        }

        return true;
    }

    void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
    {
        UIQuestDialog dlg = (UIQuestDialog) sender;
        if (result == UIWindow.WindowResult.Yes)
        {
            MessageBox.Show(dlg.quest.Define.DialogAccept);
        }
        else if (result == UIWindow.WindowResult.No)
        {
            MessageBox.Show(dlg.quest.Define.DialogDeny);
        }
    }

    public void OnQuestAccepted(Quest quest)
    {

    }
}
