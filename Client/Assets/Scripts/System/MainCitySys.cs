using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCitySys : SystemRoot {

    public MainCityWnd MainCityWnd;
    public static MainCitySys instance;
    private PlayerController PlayerController;
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
    }

    public void SetMoveDir(Vector2 dir)
    {
        if(dir==Vector2.zero)
        {
            PlayerController.SetBlend(Const.BlendIdle);
        }
        else
        {
            PlayerController.SetBlend(Const.BlendWalk);
        }
        PlayerController.Dir = dir;
    }
}
