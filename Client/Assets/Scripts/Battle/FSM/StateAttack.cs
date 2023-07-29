using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.skActionCBLst.Clear();
        entity.skMoveCBLst.Clear();
        entity.curAniState = AniState.Attack;
        entity.skillCfg = ResSvr.instance.GetSkillCfg((int)args[0]);
        PECommon.Log("Enter Attack");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        entity.ExitCurSkill();
    }

    public void Process(EntityBase entity, params object[] args)
    {
        PECommon.Log("Enter Attack");
        if(entity.entityType== EntityType.Player)
        {
            entity.canSkill = false;
        }
        entity.SkillAttack((int)args[0]);
        //entity.SetAction(1);
    }
}
