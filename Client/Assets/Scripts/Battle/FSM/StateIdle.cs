using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.skEndCB = -1;
        entity.curAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        PECommon.Log("Enter Idle");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        PECommon.Log("Exit Idle");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        PECommon.Log("Process Idle");
        if(entity.nextSkillID!=0)
        {
            entity.Attack(entity.nextSkillID);
        }
        else
        {
            if (entity.entityType == EntityType.Player)
            {
                entity.canSkill = true;
            }
            if (entity.GetDirInput() != Vector2.zero)
            {
                entity.Move();
                entity.SetDir(entity.GetDirInput());
            }
            else
            {
                entity.ctrl.SetBlend(Const.BlendIdle);
            }
        }
    }
}
