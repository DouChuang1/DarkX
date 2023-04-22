using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : SystemRoot {
    public LoginWnd loginWnd;
    public CreateRoleWnd CreateRoleWnd;
    public static LoginSys instance;
	public override void  InitSys()
	{
        base.InitSys();
        instance = this;
        Debug.Log("LoginSys Start");
    }

	public void EnterLogin()
	{
        Debug.Log("EnterLogin");
        //异步加载登录场景 显示加载进度  加载完以后打开登录
        ResSvr.AsyncLoadScene(Const.SceneLogin,OpenLoginWnd);
    }

    public void OpenLoginWnd()
    {
        loginWnd.SetWndState(true);
        AudioSvc.PlayBGMusic(Const.BGLogin);

        GameRoot.AddTips("Loading to Login Scene");
    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);
        if(msg.rspLogin.PlayerData.name=="")
        {
            CreateRoleWnd.SetWndState();
        }
        else
        {
            //进入主城
        }
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);
        CreateRoleWnd.SetWndState(false);
    }
}
