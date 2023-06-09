﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IState
{
    void Enter(EntityBase entity,params object[] args);

    void Process(EntityBase entity, params object[] args);

    void Exit(EntityBase entity, params object[] args);
}

public enum AniState
{
    None,
    Born,
    Idle,
    Move,
    Attack,
    Die,
    Hit
}
