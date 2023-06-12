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
    }

    public void ChangeState(EntityBase entity,AniState state)
    {
        if(entity.curAniState==state)
        {
            return;
        }

        if(fsm.ContainsKey(state))
        {
            if(entity.curAniState!=AniState.None)
            {
                fsm[entity.curAniState].Exit(entity);
            }
            fsm[state].Enter(entity);
            fsm[state].Process(entity);
        }
    }
}
