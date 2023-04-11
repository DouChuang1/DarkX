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

		AudioSvc audioSvc = GetComponent<AudioSvc>();
		audioSvc.InitSvc();
		//初始化系统模块
		LoginSys loginSys = GetComponent<LoginSys>();
		loginSys.InitSys();

		//进入登录场景
		loginSys.EnterLogin();
	}

	public static void AddTips(string tip)
	{
		Instance.dynamicWnd.AddTips(tip);
	}
}
