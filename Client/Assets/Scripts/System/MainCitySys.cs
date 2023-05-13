using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot {

    public MainCityWnd MainCityWnd;
    public GuideWnd GuideWnd;
    public InfoWnd InfoWnd;
    public static MainCitySys instance;
    private PlayerController PlayerController;
    private Transform charCamTrans;

    private AutoGuideCfg AutoGuideCfg;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;
    private bool isNavGuide = false;
    public override void InitSys()
    {
        base.InitSys();
        instance = this;
        Debug.Log("MainCitySys Start");
    }

    public void EnterMainCity()
    {
        MapCfg mapCfg = ResSvr.GetMapCfgData(Const.MainCityMapID);
        ResSvr.AsyncLoadScene(mapCfg.sceneName, () =>
         {
             Debug.Log("Enter Main City");
             LoadPlayer(mapCfg);
             MainCityWnd.SetWndState(true);
             AudioSvc.PlayBGMusic(Const.BGMainCity);

             GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
             MainCityMap mainCityMap = map.GetComponent<MainCityMap>();
             npcPosTrans = mainCityMap.NpcPosTrans;
             if(charCamTrans)
             {
                 charCamTrans.gameObject.SetActive(false);
             }
         });
    }

    void LoadPlayer(MapCfg mapCfg)
    {
        GameObject player = ResSvr.LoadPrefab(PathDefine.AssissnCityPlayerPrefab, true);
        player.transform.position = mapCfg.playerBornPos;
        player.transform.localEulerAngles = mapCfg.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Camera.main.transform.position = mapCfg.mainCamPos;
        Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

        PlayerController = player.GetComponent<PlayerController>();
        PlayerController.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if (dir==Vector2.zero)
        {
            PlayerController.SetBlend(Const.BlendIdle);
        }
        else
        {
            PlayerController.SetBlend(Const.BlendWalk);
        }
        PlayerController.Dir = dir;
    }

    public void OpenInfoWnd()
    {
        StopNavTask();
        if (charCamTrans==null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }

        charCamTrans.localPosition = PlayerController.transform.position + PlayerController.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + PlayerController.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        InfoWnd.SetWndState(true);
    }

    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
        }
        InfoWnd.SetWndState(false);
    }

    private float startRotate = 0;
    public void SetStartRotate()
    {
        startRotate = PlayerController.transform.localEulerAngles.y;
    }

    public void SetPlayerRotate(float rotate)
    {
        PlayerController.transform.localEulerAngles = new Vector3(0, startRotate + rotate, 0);
    }

    public void RunTask(AutoGuideCfg autoGuideCfg)
    {
        if(autoGuideCfg!=null)
        {
            AutoGuideCfg = autoGuideCfg;
        }
        if(AutoGuideCfg.npcID!=-1)
        {
            float dis = Vector3.Distance(PlayerController.transform.position, npcPosTrans[AutoGuideCfg.npcID].position);
            if(dis<=0.5f)
            {
                nav.enabled = true;
                isNavGuide = false;
                nav.isStopped = true;
                PlayerController.SetBlend(Const.BlendIdle);
                nav.enabled = false;
                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Const.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[AutoGuideCfg.npcID].position);
                PlayerController.SetBlend(Const.BlendWalk);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(PlayerController.transform.position, npcPosTrans[AutoGuideCfg.npcID].position);
        if (dis <= 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            PlayerController.SetBlend(Const.BlendIdle);
            nav.enabled = false;
            OpenGuideWnd();
        }
    }
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            nav.isStopped = true;
            nav.enabled = false;
            PlayerController.SetBlend(Const.BlendIdle);
        }
    }


    private void Update()
    {
        if(isNavGuide)
        {
            IsArriveNavPos();
            PlayerController.SetCam();
        }
    }

    public AutoGuideCfg GetCurTaskData()
    {
        return AutoGuideCfg;
    }

    public void OpenGuideWnd()
    {
        GuideWnd.SetWndState(true);
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;

        GameRoot.AddTips(Const.Color("任务奖励 金币+" + AutoGuideCfg.coin + " 经验奖励 经验+" + AutoGuideCfg.exp, TxtColor.Blue));

        switch(AutoGuideCfg.actID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }

        GameRoot.Instance.SetPlayerDataByGuide(data);
        MainCityWnd.RefreshUI();
    }
}
