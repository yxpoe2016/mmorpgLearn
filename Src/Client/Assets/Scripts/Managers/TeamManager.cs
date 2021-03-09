using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : Singleton<TeamManager> {

	// Use this for initialization
    public void Init()
    {

    }

    public void UpdateTeamInfo(NTeamInfo team)
    {
        User.Instance.TeamInfo = team;
        ShowTeamUI(team != null);
    }

    public void ShowTeamUI(bool show)
    {
        if (UIMain.Instance != null)
        {
            UIMain.Instance.ShowTeamUI(show);
        }
    }
}
