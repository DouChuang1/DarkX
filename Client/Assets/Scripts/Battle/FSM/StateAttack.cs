using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curAniState = AniState.Attack;
        PECommon.Log("Enter Attack");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        entity.canControl = true;
        entity.SetAction(-1);
        PECommon.Log("Exit Attack");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        PECommon.Log("Enter Attack");
        entity.SkillAttack((int)args[0]);
        //entity.SetAction(1);
    }
}
