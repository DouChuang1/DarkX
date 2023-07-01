using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateBorn : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curAniState = AniState.Born;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Const.ActionBorn);
        TimerSvc.Instance.AddTimeTask((int id) =>
        {
            entity.SetAction(Const.ActionDefault);
        }, 500);
    }
}
