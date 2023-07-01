using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StateMgr:MonoBehaviour
{
    public Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();
    public void Init()
    {
        fsm.Add(AniState.Idle, new StateIdle());
        fsm.Add(AniState.Move, new StateMove());
        fsm.Add(AniState.Attack, new StateAttack());
        fsm.Add(AniState.Born, new StateBorn());
        fsm.Add(AniState.Die, new StateDie());
        fsm.Add(AniState.Hit, new StateHit());
    }

    public void ChangeState(EntityBase entity,AniState state,params object[] args)
    {
        if(entity.curAniState==state)
        {
            return;
        }

        if(fsm.ContainsKey(state))
        {
            if(entity.curAniState!=AniState.None)
            {
                fsm[entity.curAniState].Exit(entity, args);
            }
            fsm[state].Enter(entity, args);
            fsm[state].Process(entity,args);
        }
    }
}
