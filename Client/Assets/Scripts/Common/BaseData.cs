using System;
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
