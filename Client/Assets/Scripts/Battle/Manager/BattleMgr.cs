using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleMgr:MonoBehaviour
{
    private ResSvr ResSvr;
    private StateMgr StateMgr;
    private SkillMgr SkillMgr;
    private MapMgr MapMgr;
    private EntityBase entityPlayer;

    public void Init(int mapId,Action cb)
    {
        ResSvr = ResSvr.instance;
        StateMgr = gameObject.AddComponent<StateMgr>();
        StateMgr.Init();
        SkillMgr = gameObject.AddComponent<SkillMgr>();
        SkillMgr.Init();

        MapCfg mapCfg = ResSvr.GetMapCfgData(mapId);
        ResSvr.AsyncLoadScene(mapCfg.sceneName, () =>
         {
             GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
             MapMgr = map.GetComponent<MapMgr>();
             MapMgr.Init(mapId);

             map.transform.localPosition = Vector3.zero;
             map.transform.localScale = Vector3.one;

             Camera.main.transform.position = mapCfg.mainCamPos;
             Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;
             LoadPlayer(mapCfg);
             entityPlayer.Idle();
             AudioSvc.Instance.PlayBGMusic(Const.BGHuangYe);
             if(cb!=null)
             {
                 cb();
             }
         });
    }

    public void LoadPlayer(MapCfg mapCfg)
    {
        GameObject player = ResSvr.LoadPrefab(PathDefine.AssissnBattlePlayerPrefab);

        player.transform.position = mapCfg.playerBornPos;
        player.transform.localScale = Vector3.one;
        player.transform.localEulerAngles = mapCfg.playerBornRote;
        entityPlayer = new EntityPlayer();
        entityPlayer.stateMgr = StateMgr;
        entityPlayer.skillMgr = SkillMgr;
        entityPlayer.battleMgr = this;
        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        entityPlayer.playCtrl = playerCtrl;
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if(!entityPlayer.canControl)
        {
            return;
        }
        if(dir==Vector2.zero)
        {
            entityPlayer.Idle();
        }
        else
        {
            entityPlayer.Move();
            entityPlayer.SetDir(dir);
        }
    }

    public void ReleaseNormalAttack()
    {

    }

    public void ReleaseSkill1()
    {
        entityPlayer.Attack(101);
    }
    public void ReleaseSkill2()
    {

    }
    public void ReleaseSkill3()
    {

    }

    public void ReqReleaseSkill(int index)
    {
        switch(index)
        {
            case 0:
                ReleaseNormalAttack();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }

    public Vector2 GetDirInput()
    {
        return BattleSys.instance.GetDirInput();
    }
}
