using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SystemRoot
{

    public static BattleSys instance;
    public PlayerCtrlWnd PlayerCtrlWnd;
    public BattleMgr battleMgr;
    public override void InitSys()
    {
        base.InitSys();
        instance = this;
        Debug.Log("BattleSys Start");
    }

    public void StartBattle(int mapId)
    {
        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId,()=>
        {
            SetPlayerCtrlWndState(true);
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
}
