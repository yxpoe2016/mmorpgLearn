using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;

    public Text[] targets;

    public Text description;

    public UIIconItem rewardItems;

    public Text rewardMoney;

    public Text rewarExp;

	// Use this for initialization
	void Start () {
		
	}

    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        if (quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
        }
        else
        {
            if (quest.Info.Status == QuestStatus.Complated)
                this.description.text = quest.Define.DialogFinish;
        }

        if (this.rewardMoney!=null)
        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        if (this.rewarExp != null)
            this.rewarExp.text = quest.Define.RewardExp.ToString();

        // foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        // {
        //     fitter.SetLayoutVertical();
        // }
    }
}
