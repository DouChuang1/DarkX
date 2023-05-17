using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour {

	public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;
    public static GameRoot Instance = null;
	// Use this for initialization
	void Start () {
		Debug.Log("Game Start--------");
		Instance= this;
		DontDestroyOnLoad(gameObject);
		ClearUI();

        Init();
	}

	void ClearUI()
	{
		Transform canvas = transform.Find("Canvas");
		for(int i=0;i<canvas.childCount;i++)
		{
			canvas.GetChild(i).gameObject.SetActive(false);
		}
		dynamicWnd.SetWndState(true);
	}
	
	void Init()
	{
		//初始化服务模块
		ResSvr res = GetComponent<ResSvr>();
		res.Init();

        NetSvc netSvc = GetComponent<NetSvc>();
        netSvc.InitSvc();

		AudioSvc audioSvc = GetComponent<AudioSvc>();
		audioSvc.InitSvc();
		//初始化系统模块
		LoginSys loginSys = GetComponent<LoginSys>();
		loginSys.InitSys();

        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();

        //进入登录场景
        loginSys.EnterLogin();
	}

	public static void AddTips(string tip)
	{
		Instance.dynamicWnd.AddTips(tip);
	}

    private PlayerData playerData = null;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }

    public void SetPlayerData(RspLogin data)
    {
        this.playerData = data.PlayerData;
    }
    public void SetPlayerName(string name)
    {
        playerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        playerData.lv = data.lv;
        playerData.coin = data.coin;
        playerData.exp = data.exp;
        playerData.guideid = data.guideid;
    }

    public void SetPlayerDataByStrong(RspStrong data)
    {
        playerData.coin = data.coin;
        playerData.crystal = data.crystal;
        playerData.hp = data.hp;
        playerData.ad = data.ad;
        playerData.ap = data.ap;
        playerData.addef = data.addef;
        playerData.apdef = data.apdef;
        playerData.strong = data.strongArr;
    }

}
