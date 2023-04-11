using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot {

	public InputField account;
	public InputField password;

	public Button btnEnter;
	public Button btnNotice;

	public override void InitWnd()
	{
		base.InitWnd();
		if(PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
		{
			account.text = PlayerPrefs.GetString("Acct");
			password.text = PlayerPrefs.GetString("Pass");
		}
		else
		{
			account.text = "";
			password.text = "";
		}
	}

	public void ClickLoginBtn()
	{
        audioSvc.PlayUIAudio(Const.UILogin);

		string acc = account.text;
		string pass = password.text;

		if(acc!=null && pass!=null )
		{
			PlayerPrefs.SetString("Acct", acc);
			PlayerPrefs.SetString ("Password", pass);

			LoginSys.instance.RspLogin();
		}
		else
		{
			GameRoot.AddTips("账号或者密码为空");
		}
		
	}


}
