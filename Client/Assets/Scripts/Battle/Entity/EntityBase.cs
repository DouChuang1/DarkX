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
    public PlayerController playCtrl = null;
    public bool canControl = true;
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
        if(playCtrl!=null)
        {
            playCtrl.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if (playCtrl != null)
        {
            playCtrl.Dir = dir;
        }
    }

    public virtual void SetAction(int act)
    {
        if (playCtrl != null)
        {
            playCtrl.SetAction(act);
        }
    }

    public virtual void AttackEffect(int skillID)
    {
        skillMgr.AttackEffect(this, skillID);
    }

    public virtual void SetFX(string name,float destroyTime)
    {
        if (playCtrl != null)
        {
            playCtrl.SetFX(name,destroyTime);
        }
    }

    public virtual void SetSkillMoveState(bool move,float speed=0)
    {
        if (playCtrl != null)
        {
            playCtrl.SetSkillMoveState(move, speed);
        }
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
    }
}
