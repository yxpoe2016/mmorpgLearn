﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildPopCreate : UIWindow
{

    public InputField inputName;
    public InputField notice;
    // Use this for initialization
    private void Start ()
    {
        GuildService.Instance.OnGuildCreateResult = OnGuildCreated;
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildCreateResult = null;
    }

    public override void OnYesClick()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            MessageBox.Show("请输入公会名称", "错误", MessageBoxType.Error);
            return;
        }

        if (inputName.text.Length < 4 || inputName.text.Length > 10)
        {
            MessageBox.Show("公会名称为4-10个字符", "错误", MessageBoxType.Error);
            return;
        }
        if (string.IsNullOrEmpty(notice.text))
        {
            MessageBox.Show("请输入公会宣言", "错误", MessageBoxType.Error);
            return;
        }

        GuildService.Instance.SendGuildCreate(inputName.text,notice.text);
    }

    private void OnGuildCreated(bool result)
    {
        if (result)
            this.Close(WindowResult.Yes);
    }
}
