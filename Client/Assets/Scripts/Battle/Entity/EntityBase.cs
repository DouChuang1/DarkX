using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EntityBase
{
    public AniState curAniState = AniState.None;

    public StateMgr stateMgr = null;

    public PlayerController playCtrl = null;

    public void Move()
    {
        stateMgr.ChangeState(this, AniState.Move);
    }

    public void Idle()
    {
        stateMgr.ChangeState(this, AniState.Idle);
    }
}
