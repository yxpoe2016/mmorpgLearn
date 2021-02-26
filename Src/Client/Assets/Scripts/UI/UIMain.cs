using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{

    public Text avatarName;

    public Text avatarLevel;

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

    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
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
}
