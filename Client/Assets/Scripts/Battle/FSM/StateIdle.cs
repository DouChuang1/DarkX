using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateIdle : IState
{
    public void Enter(EntityBase entity)
    {
        entity.curAniState = AniState.Idle;
        PECommon.Log("Enter Idle");
    }

    public void Exit(EntityBase entity)
    {
        PECommon.Log("Exit Idle");
    }

    public void Process(EntityBase entity)
    {
        PECommon.Log("Process Idle");
    }
}
