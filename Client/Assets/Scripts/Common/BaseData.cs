﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class BaseData<T>
{
    public int ID;
}

public class MapCfg:BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int startlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class TaskRewardCfg :BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class SkillCfg:BaseData<SkillCfg>
{
    public string skillName;
    public int skillTime;
    public int aniAction;
    public string fx;
    public List<int> skillMoveLst;
}

public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public int moveTime;
    public float moveDis;
    public int delayTime;
}