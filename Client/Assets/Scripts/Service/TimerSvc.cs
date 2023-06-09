﻿using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSvc : SystemRoot {

    public static TimerSvc Instance = null;

    private PETimer pt;

    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();

        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });
    }

    public void Update()
    {
        pt.Update();
    }

    public int AddTimeTask(Action<int> callback,double delay,PETimeUnit timeUnit = PETimeUnit.Millisecond,int count=1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }
}
