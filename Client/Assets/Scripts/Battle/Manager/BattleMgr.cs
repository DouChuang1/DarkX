using PEProtocol;
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
    private MapCfg mapCfg;
    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();
    public void Init(int mapId,Action cb)
    {
        ResSvr = ResSvr.instance;
        StateMgr = gameObject.AddComponent<StateMgr>();
        StateMgr.Init();
        SkillMgr = gameObject.AddComponent<SkillMgr>();
        SkillMgr.Init();

        mapCfg = ResSvr.GetMapCfgData(mapId);
        ResSvr.AsyncLoadScene(mapCfg.sceneName, () =>
         {
             GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
             MapMgr = map.GetComponent<MapMgr>();
             MapMgr.Init(this);

             map.transform.localPosition = Vector3.zero;
             map.transform.localScale = Vector3.one;

             Camera.main.transform.position = mapCfg.mainCamPos;
             Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;
             LoadPlayer(mapCfg);
             entityPlayer.Idle();
             ActiveCurrentBatchMonsters();
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

        PlayerData pd = GameRoot.Instance.PlayerData;
        BattleProps props = new BattleProps
        {
            hp = pd.hp,
            ad = pd.ad,
            ap = pd.ap,
            addef = pd.addef,
            apdef = pd.apdef,
            dodge = pd.dodge,
            pierce = pd.pierce,
            critical = pd.critical
        };
        entityPlayer = new EntityPlayer();
        entityPlayer.SetBattleProps(props);
        entityPlayer.stateMgr = StateMgr;
        entityPlayer.skillMgr = SkillMgr;
        entityPlayer.battleMgr = this;
        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        entityPlayer.ctrl = playerCtrl;
    }

    public void LoadMonsterByWaveID(int waveId)
    {
        for(int i=0;i<mapCfg.monsterList.Count;i++)
        {
            MonsterData md = mapCfg.monsterList[i];
            if(md.mWave==waveId)
            {
                GameObject m = ResSvr.LoadPrefab(md.mCfg.resPath, true);
                m.transform.localPosition = md.mBornPos;
                m.transform.localEulerAngles = md.mBornRote;
                m.transform.localScale = Vector3.one;
                m.name = "m" + md.mWave + "_" + md.mIndex;

                EntityMonster em = new EntityMonster
                {
                    battleMgr = this,
                    stateMgr = StateMgr,
                    skillMgr = SkillMgr
                };
                em.md = md;
                em.SetBattleProps(md.mCfg.bps);
                MonsterController mc = m.GetComponent<MonsterController>();
                mc.Init();
                em.ctrl = mc;
                m.SetActive(false);
                monsterDic.Add(m.name, em);
            }
        }
    }

    public void ActiveCurrentBatchMonsters()
    {
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            foreach(var item in monsterDic)
            {
                item.Value.ctrl.gameObject.SetActive(true);
                item.Value.Born();
                TimerSvc.Instance.AddTimeTask((int id) =>
                {
                    item.Value.Idle();
                }, 1000);
            }
        },500);
    }

    public List<EntityMonster> GetEntityMonsters()
    {
        List<EntityMonster> monsterLst = new List<EntityMonster>();
        foreach(var it in monsterDic)
        {
            monsterLst.Add(it.Value);
        }
        return monsterLst;  
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
