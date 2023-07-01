using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curAniState = AniState.Die;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Const.ActionDie);
        TimerSvc.Instance.AddTimeTask((int id) =>
        {
            entity.ctrl.gameObject.SetActive(false);
        }, 5000);
    }
}