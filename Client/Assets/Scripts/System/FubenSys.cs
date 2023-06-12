using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FubenSys : SystemRoot {

    public FubenWnd  FubenWnd;
    public static FubenSys instance;
    public override void InitSys()
    {
        base.InitSys();
        instance = this;
        Debug.Log("FubenSys Start");
    }

    public void EnterFuben()
    {
        OpenFubenWnd();
    }

    public void OpenFubenWnd()
    {
        FubenWnd.SetWndState(true);
    }

    public void RspFBFight(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerDataByFBStart(msg.rspFBFight);

        MainCitySys.instance.MainCityWnd.SetWndState(false);
        FubenWnd.SetWndState(false);
        BattleSys.instance.StartBattle(msg.rspFBFight.fbid);
    }
}
