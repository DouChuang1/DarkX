using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateMove : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curAniState = AniState.Move;
        PECommon.Log("Enter Walk");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        PECommon.Log("Exit Walk");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        PECommon.Log("Process Walk");
        entity.playCtrl.SetBlend(Const.BlendWalk);
    }
}
