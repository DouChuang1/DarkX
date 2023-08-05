using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SystemRoot
{

    public static BattleSys instance;
    public PlayerCtrlWnd PlayerCtrlWnd;
    public BattleMgr battleMgr;
    public BattleEndWnd BattleEndWnd;
    private int fbid;
    private double startTime;
    public override void InitSys()
    {
        base.InitSys();
        instance = this;
        Debug.Log("BattleSys Start");
    }

    public void StartBattle(int mapId)
    {
        fbid = mapId;
        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId,()=>
        {
            SetPlayerCtrlWndState(true);
            startTime = TimerSvc.GetNowTime();
        });
       
    }

    public void SetPlayerCtrlWndState(bool isActive=true)
    {
        PlayerCtrlWnd.SetWndState(isActive);
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return PlayerCtrlWnd.currentDir;
    }

    public void EndBattle(bool isWin,int restHP)
    {
        PlayerCtrlWnd.SetWndState(false);
        GameRoot.Instance.dynamicWnd.RmvAllHPItemInfo();
        if(isWin)
        {
            double endTime = TimerSvc.GetNowTime();
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqFBFightEnd,
                reqFBFightEnd = new ReqFBFightEnd
                {
                    win = isWin,
                    fbid = fbid,
                    resthp = restHP,
                    costtime = (int)((endTime - startTime) / 1000)
                }
            };
            NetSvc.SendMsg(msg);
        }
        else
        {
            SetBattleEndWndState(FBEndType.Lose, true);
        }
    }

    public void RspFBFightEnd(GameMsg msg)
    {
        RspFBFightEnd data = msg.rspFBFightEnd;
        GameRoot.Instance.SetPlayerDataByFBEnd(data);
        BattleEndWnd.SetBattleEndData(data.fbid, data.costtime, data.resthp);
        SetBattleEndWndState(FBEndType.Win);
    }

    public void SetBattleEndWndState(FBEndType endType, bool isActive=true)
    {
        BattleEndWnd.SewtWndType(endType);
        BattleEndWnd.SetWndState(isActive);
    }

    public void DistroyBattle()
    {
        SetPlayerCtrlWndState(false);
        SetBattleEndWndState(FBEndType.None, false);
        GameRoot.Instance.dynamicWnd.RmvAllHPItemInfo();
        Destroy(battleMgr.gameObject);
    }
}
