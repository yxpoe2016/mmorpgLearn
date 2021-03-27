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

    public Button navButton;

    private int npc = 0;
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

        if (quest.Info == null)
        {
            this.npc = quest.Define.AcceptNPC;
        }
        else if (quest.Info.Status == QuestStatus.Complated)
        {
            this.npc = quest.Define.SubmitNPC;
        }
        this.navButton.gameObject.SetActive(this.npc>0);
        // foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        // {
        //     fitter.SetLayoutVertical();
        // }
    }

    public void onClickNav()
    {
        Vector3 pos = NpcManager.Instance.GetNpcPosition(this.npc);
        User.Instance.CurrentCharacterObject.StarNav(pos);
        UIManager.Instance.Close<UIQuestSystem>();
    }
}
