using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Network;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

public class UserService : Singleton<UserService>, IDisposable
{
    public UnityAction<Result, string> OnRegister;
    public UnityAction<Result, string> OnLogin;
    public UnityAction<Result, string> OnCharacterCreate;
    private bool connected;
    private NetMessage pendingMessage;

    public UserService()
    {
        NetClient.Instance.OnConnect += OnGameServerConnect;
        NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
        MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
        MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
        MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
        MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnGameEnter);
        MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnGameLeave);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
        MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
        MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
        MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnGameEnter);
        MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(this.OnGameLeave);
        NetClient.Instance.OnConnect -= OnGameServerConnect;
        NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
    }

    public void Init()
    {

    }

    public void ConnectToServer()
    {
        Debug.Log("ConnectToServer() Start");
        NetClient.Instance.Init("127.0.0.1",8000);
        NetClient.Instance.Connect();
    }

    void OnGameServerConnect(int result, string reason)
    {
        Log.InfoFormat("LoadingMessager::OnGameServerConnect :{0} reason:{1}",result,reason);
        if (NetClient.Instance.Connected)
        {
            this.connected = true;
            if (this.pendingMessage!=null)
            {
                NetClient.Instance.SendMessage(this.pendingMessage);
                this.pendingMessage = null;
            }
        }
        else
        {
            if (!this.DisconnectNotify(result,reason))
            {
                MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
            }
        }
    }

    public void OnGameServerDisconnect(int result, string reason)
    {
        this.DisconnectNotify(result, reason);
        return;
    }

    bool DisconnectNotify(int result, string reason)
    {
        if (this.pendingMessage!=null)
        {
            if (this.pendingMessage.Request.userRegister!=null)
            {
                if (this.OnRegister!= null)
                {
                    this.OnRegister(Result.Failed, string.Format("服务器断开!\n RESULT: {0} ERROR:{1}", result, reason));
                }
            }
            return true;
        }
        return false;
    }

    public void SendRegister(string user, string psw)
    {
        Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.userRegister = new UserRegisterRequest();
        message.Request.userRegister.User = user;
        message.Request.userRegister.Passward = psw;

        if (this.connected && NetClient.Instance.Connected)
        {
            this.pendingMessage = null;
            NetClient.Instance.SendMessage(message);
        }
        else
        {
            this.pendingMessage = message;
            this.ConnectToServer();
        }

    }

    private void OnUserRegister(object sender, UserRegisterResponse response)
    {
        Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);
        if (this.OnRegister!=null)
        {
            this.OnRegister(response.Result, response.Errormsg);
        }
    }

    public void SendLogin(string user, string psw)
    {
        Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.userLogin = new UserLoginRequest();
        message.Request.userLogin.User = user;
        message.Request.userLogin.Passward = psw;

        if (this.connected && NetClient.Instance.Connected)
        {
            this.pendingMessage = null;
            NetClient.Instance.SendMessage(message);
        }
        else
        {
            this.pendingMessage = message;
            this.ConnectToServer();
        }

    }

    private void OnUserLogin(object sender, UserLoginResponse response)
    {
        Debug.LogFormat("OnUserLogin:{0} [{1}]", response.Result, response.Errormsg);
        if (response.Result == Result.Success)
        {
            User.Instance.SetupUserInfo(response.Userinfo);
        }
        if (this.OnLogin != null)
        {
            this.OnLogin(response.Result, response.Errormsg);
        }
    }

    public void SendCharecterCreate(string charName, CharacterClass cls)
    {
        Debug.LogFormat("CharecterCreateRequest::charName :{0} cls:{1}", charName, cls);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.createChar = new UserCreateCharacterRequest();
        message.Request.createChar.Name = charName;
        message.Request.createChar.Class = cls;

        if (this.connected && NetClient.Instance.Connected)
        {
            this.pendingMessage = null;
            NetClient.Instance.SendMessage(message);
        }
        else
        {
            this.pendingMessage = message;
            this.ConnectToServer();
        }

    }

    void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)
    {
        Debug.LogFormat("OnUserCreateCharacter:{0} [{1}]", response.Result, response.Errormsg);

        if (response.Result == Result.Success)
        {
            User.Instance.Info.Player.Characters.Clear();
            User.Instance.Info.Player.Characters.AddRange(response.Characters);
        }

        if (this.OnCharacterCreate !=null)
        {
            OnCharacterCreate(response.Result, response.Errormsg);
        }
    }

    public void SendGameEnter(int characterIdx)
    {
        Debug.LogFormat("characterIdx:{0}", characterIdx);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.gameEnter = new UserGameEnterRequest();
        message.Request.gameEnter.characterIdx = characterIdx;
        NetClient.Instance.SendMessage(message);

    }

    void OnGameEnter(object sender, UserGameEnterResponse response)
    {
        Debug.LogFormat("OnGameEnter:{0} [{1}]", response.Result, response.Errormsg);

        if (response.Result == Result.Success)
        {

        }
    }


    public void SendGameLeave()
    {
        Debug.Log("UserGameLeaveRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.gameLeave = new UserGameLeaveRequest();
        NetClient.Instance.SendMessage(message);
    }

    void OnGameLeave(object sender, UserGameLeaveResponse response)
    {
        Debug.LogFormat("OnGameLeave:{0} [{1}]", response.Result, response.Errormsg);
    }

}
