using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EntityBase
{
    public AniState curAniState = AniState.None;

    public StateMgr stateMgr = null;
    public SkillMgr skillMgr = null;
    public BattleMgr battleMgr = null;
    public Controller ctrl = null;
    public bool canControl = true;
    public bool canSkill = true;
    public EntityType entityType = EntityType.None;
    public EntityState entityState = EntityState.None;
    private string name;  
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
    private BattleProps props;
    public BattleProps Props
    {
        get
        {
            return props;
        }
        protected set
        {
            props = value;
        }
    }

    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            PEProtocol.PECommon.Log("hp change:" + hp + "to" + value);
            SetHpValue(hp, value);
            hp = value;
        }
    }

    public Queue<int> comboQue = new Queue<int>();
    public int nextSkillID = 0;
    public SkillCfg skillCfg;
    public List<int> skMoveCBLst = new List<int>();
    public List<int> skActionCBLst = new List<int>();
    public int skEndCB = -1;
    public void Hit()
    {
        stateMgr.ChangeState(this, AniState.Hit, null);
    }

    public void Die()
    {
        stateMgr.ChangeState(this, AniState.Die, null);
    }

    public virtual void TickAILogic()
    {

    }
    public void Born()
    {
        stateMgr.ChangeState(this, AniState.Born, null);
    }
    public void Move()
    {
        stateMgr.ChangeState(this, AniState.Move,null);
    }

    public void Idle()
    {
        stateMgr.ChangeState(this, AniState.Idle, null);
    }

    public void Attack(int skillID)
    {
        stateMgr.ChangeState(this, AniState.Attack,skillID);
    }

    public virtual void SetBlend(float blend)
    {
        if(ctrl != null)
        {
            ctrl.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if (ctrl != null)
        {
            ctrl.Dir = dir;
        }
    }

    public virtual void SetAction(int act)
    {
        if (ctrl != null)
        {
            ctrl.SetAction(act);
        }
    }

    public virtual void SkillAttack(int skillID)
    {
        skillMgr.SkillAttack(this, skillID);
    }

    public virtual void SetFX(string name,float destroyTime)
    {
        if (ctrl != null)
        {
            ctrl.SetFX(name,destroyTime);
        }
    }

    public virtual void SetSkillMoveState(bool move,float speed=0)
    {
        if (ctrl != null)
        {
            ctrl.SetSkillMoveState(move, speed);
        }
    }

    public virtual void SetAtkRotation(Vector2 dir,bool offset=false)
    {
        if (ctrl != null)
        {
            if(offset)
            {
                ctrl.SetAtkRotationCam(dir);
            }
            else
            {
                ctrl.SetAtkRotationLocal(dir);
            }
            
        }
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
    }

    public virtual Vector3 GetPos()
    {
        return ctrl.transform.position;
    }

    public virtual Transform GetTrans()
    {
        return ctrl.transform;
    }

    public virtual void SetBattleProps(BattleProps props)
    {
        Hp = props.hp;
        this.Props = props;
    }


    public virtual void SetDodge()
    {
        if (ctrl != null)
        {
            GameRoot.Instance.dynamicWnd.SetDodge(Name);
        }
    }

    public virtual void SetCritical(int critical)
    {
        if (ctrl != null)
        {
            GameRoot.Instance.dynamicWnd.SetCritical(Name, critical);
        }
    }

    public virtual void SetHurt(int hurt)
    {
        if (ctrl != null)
        {
            GameRoot.Instance.dynamicWnd.SetHurt(Name, hurt);
        }
    }

    public virtual void SetHpValue(int oldValue,int newValue)
    {
        if(ctrl!=null)
        {
            GameRoot.Instance.dynamicWnd.SetHpValue(Name, oldValue, newValue);
        }
    }

    public void SetCtrl(Controller controller)
    {
        ctrl = controller;
    }

    public void SetActive(bool active)
    {
        ctrl.gameObject.SetActive(active);
    }

    public void ExitCurSkill()
    {
        canControl = true;
        if(!skillCfg.isBreak)
        {
            entityState = EntityState.None;
        }
        if(skillCfg.isCombo)
        {
            if (comboQue.Count > 0)
            {
                nextSkillID = comboQue.Dequeue();
            }
            else
            {
                nextSkillID = 0;
            }
        }
        skillCfg = null;
        SetAction(-1);
    }

    public virtual Vector2 CalcTargetDir()
    {
        return Vector2.zero;
    }

    public AudioSource GetAudio()
    {
        return ctrl.GetComponent<AudioSource>();
    }

    public void RemoveMoveCB(int tid)
    {
        int index = -1;
        for(int i=0;i<skMoveCBLst.Count;i++)
        {
            if(skMoveCBLst[i]==tid)
            {
                index = i;
                break;
            }
        }

        if(index!=-1)
        {
            skMoveCBLst.Remove(index);
        }
    }

    public void RemoveActionCB(int tid)
    {
        int index = -1;
        for (int i = 0; i < skActionCBLst.Count; i++)
        {
            if (skActionCBLst[i] == tid)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            skActionCBLst.Remove(index);
        }
    }

    public virtual bool GetBreakState()
    {
        return true;
    }
}
