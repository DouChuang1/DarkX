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
    public EntityPlayer entityPlayer;
    private MapCfg mapCfg;
    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();
    public bool isPauseGame = false;
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
    public bool triggerCheck = true;
    public void Update()
    {
        foreach(var item in monsterDic)
        {
            EntityMonster em = item.Value;
            em.TickAILogic();
        }
        if(MapMgr!=null)
        {
            if(triggerCheck && monsterDic.Count==0)
            {
                bool isExist = MapMgr.SetNextTriggerOn();
                triggerCheck = false;
                if(!isExist)
                {
                    //关卡结束 
                    EndBattle(true, entityPlayer.Hp);
                }
            }
        }
    }

    public void EndBattle(bool isWin,int restHP)
    {
        isPauseGame = true;
        AudioSvc.Instance.StopBGMusic();
        BattleSys.instance.EndBattle(isWin, restHP);
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
        entityPlayer.Name = "Assassin";
        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        entityPlayer.SetCtrl(playerCtrl);
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
                em.Name = m.name;
                em.SetBattleProps(md.mCfg.bps);
                MonsterController mc = m.GetComponent<MonsterController>();
                mc.Init();
                em.SetCtrl(mc);
                m.SetActive(false);
                monsterDic.Add(m.name, em);
                if(md.mCfg.mType== MonsterType.Normal)
                {
                    GameRoot.Instance.dynamicWnd.AddHpItemInfo(m.name, em.ctrl.hpRoot, em.Hp);
                }
                else if(md.mCfg.mType== MonsterType.Boss)
                {
                    BattleSys.instance.PlayerCtrlWnd.SetBossHPBarState(true);
                }
            }
        }
    }

    public void ActiveCurrentBatchMonsters()
    {
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            foreach(var item in monsterDic)
            {
                item.Value.SetActive(true);
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
        if(entityPlayer.curAniState== AniState.Idle || entityPlayer.curAniState== AniState.Move)
        {
            if (dir == Vector2.zero)
            {
                entityPlayer.Idle();
            }
            else
            {
                entityPlayer.Move();
                entityPlayer.SetDir(dir);
            }
        }
        
    }

    private int[] comboArr = new int[] { 111, 112, 113, 114, 115 };
    public double lastAtkTime = 0;
    public int comboIndex = 0;
    public void ReleaseNormalAttack()
    {
        if (entityPlayer.curAniState == AniState.Attack)
        {
            double nowAttackTime = TimerSvc.Instance.GetNowTime();
            if(nowAttackTime-lastAtkTime<Const.ComboSpace && lastAtkTime!=0)
            {
                if(comboIndex!=comboArr.Length-1)
                {
                    comboIndex += 1;
                    entityPlayer.comboQue.Enqueue(comboArr[comboIndex]);
                    lastAtkTime = nowAttackTime;
                }
                else
                {
                    lastAtkTime = 0;
                    comboIndex = 0;
                }
            }
        }
        else if (entityPlayer.curAniState == AniState.Idle || entityPlayer.curAniState == AniState.Move)
        {
            comboIndex = 0;
            lastAtkTime = TimerSvc.Instance.GetNowTime();
            entityPlayer.Attack(comboArr[0]);
        }
    }

    public void ReleaseSkill1()
    {
        entityPlayer.Attack(101);
    }
    public void ReleaseSkill2()
    {
        entityPlayer.Attack(102);
    }
    public void ReleaseSkill3()
    {
        entityPlayer.Attack(103);
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

    public void RemoveMonster(string key)
    {
        EntityMonster em;
        if(monsterDic.TryGetValue(key,out em))
        {
            monsterDic.Remove(key);
            GameRoot.Instance.dynamicWnd.RmvHpItemInfo(key);
        }
    }

    public bool CanRlsSkill()
    {
        return entityPlayer.canSkill;
    }
}
