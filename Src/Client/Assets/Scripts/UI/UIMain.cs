using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{

    public Text avatarName;

    public Text avatarLevel;

    public UITeam TeamWindow;
	// Use this for initialization
    protected override void OnStart()
    {
        UpdateAvatar();
    }

    void UpdateAvatar()
    {
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
        this.avatarName.text = User.Instance.CurrentCharacter.Name;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
 
    public void OnClickTestUI()
    {
        UITest test =UIManager.Instance.Show<UITest>(this.transform);
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击" + result, "对话框", MessageBoxType.Information);
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }
    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }

    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }

    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }


    public void OnClickSkill()
    {

    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }

}
