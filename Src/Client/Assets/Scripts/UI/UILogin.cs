using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour {

    public InputField username;

    public InputField password;

    public Button buttonLogin;

    public Button buttonRegister;
    // Use this for initialization
    void Start()
    {
        buttonLogin.onClick.AddListener(OnClickLogin);
        UserService.Instance.OnLogin = OnLoginResponse;
    }

    void OnRegister()
    {

    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }

        if (string.IsNullOrEmpty(password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }

        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendLogin(username.text, password.text);
    }

    public void OnClickRegister()
    {

    }

    private void OnLoginResponse(Result result, string msg)
    {
        switch (result)
        {
            case Result.Success:
               SceneManager.Instance.LoadScene("CharSelect");
                break;
            case Result.Failed:
                MessageBox.Show(msg,"错误",MessageBoxType.Error);
                break;
            default:
                break;
        }
    }
}
