using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curAniState = AniState.Die;
        entity.RemoveSkillCB();
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Const.ActionDie);
        TimerSvc.Instance.AddTimeTask((int id) =>
        {
            if(entity.entityType== EntityType.Monster)
            {
                entity.SetActive(false);
                entity.GetCC().enabled = false;
            }
                
        }, 5000);
    }
}