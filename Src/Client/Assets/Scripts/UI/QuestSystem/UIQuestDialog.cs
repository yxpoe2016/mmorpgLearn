using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

public class UIQuestDialog : UIWindow
{
    public UIQuestInfo questInfo;

    public Quest quest;

    public GameObject openButtons;

    public GameObject submitBttons;

	// Use this for initialization
	void Start () {
		
	}

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        if (this.quest.Info == null)
        {
            openButtons.SetActive(true);
                submitBttons.SetActive(false);
        }
        else
        {
            if (this.quest.Info.Status == QuestStatus.Complated)
            {
                openButtons.SetActive(false);
                submitBttons.SetActive(true);
            }
            else
            {
                openButtons.SetActive(false);
                submitBttons.SetActive(false);
            }
        }
    }

    private void UpdateQuest()
    {
        if (this.quest != null)
        {
            if (this.questInfo != null)
            {
                this.questInfo.SetQuestInfo(quest);
            }
        }
    }
}
