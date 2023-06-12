using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateMove : IState
{
    public void Enter(EntityBase entity)
    {
        entity.curAniState = AniState.Move;
        PECommon.Log("Enter Walk");
    }

    public void Exit(EntityBase entity)
    {
        PECommon.Log("Exit Walk");
    }

    public void Process(EntityBase entity)
    {
        PECommon.Log("Process Walk");
    }
}
