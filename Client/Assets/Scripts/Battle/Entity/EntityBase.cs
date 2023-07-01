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
            hp = value;
        }
    }
    public void Hit()
    {
        stateMgr.ChangeState(this, AniState.Hit, null);
    }

    public void Die()
    {
        stateMgr.ChangeState(this, AniState.Die, null);
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

}
