using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCity : MonoBehaviour
{

    public Text avatarName;

    public Text avatarLevel;

	// Use this for initialization
	void Start ()
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
}
