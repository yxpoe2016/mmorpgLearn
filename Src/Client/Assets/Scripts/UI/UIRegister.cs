using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIRegister : MonoBehaviour
{

    public InputField username;

    public InputField password;

    public InputField passwordConfirm;

    public Button buttonRegister;

    public GameObject loginObj;

	// Use this for initialization
	void Start () {
        buttonRegister.onClick.AddListener(OnClickRegister);
        UserService.Instance.OnRegister = OnRegisterResponse;

    }

    void OnRegister()
    {

    }

    public void OnClickRegister()
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

        if (string.IsNullOrEmpty(passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }

        if (password.text!=passwordConfirm.text)
        {
            MessageBox.Show("两次输入密码不一致");
            return;
        }

        UserService.Instance.SendRegister(username.text,password.text);
    }

    private void OnRegisterResponse(Result result,string msg)
    {
        switch (result)
        {
            case Result.Success:
                loginObj.SetActive(true);
                transform.gameObject.SetActive(false);
                break;
            case Result.Failed:
                MessageBox.Show("注册失败");
                break;
            default:
                break;
        }
    }

    public void backBtn()
    {
        loginObj.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}
