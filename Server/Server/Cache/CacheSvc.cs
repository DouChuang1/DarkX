using PEProtocol;
using System;
using System.Collections.Generic;
using System.Text;

public class CacheSvc
{
    private static CacheSvc instance = null;
    public static CacheSvc Instance { get { if (instance == null) instance = new CacheSvc(); return instance; } }

    public Dictionary<string, SeverSession> onLineAcctDict = new Dictionary<string, SeverSession>();
    public Dictionary<SeverSession, PlayerData> onLineSessionDict = new Dictionary<SeverSession, PlayerData>();

    public void Init()
    {

    }

    public bool IsAcctOnline(string acct)
    {
        return onLineAcctDict.ContainsKey(acct);
    }

    public PlayerData GetPlayerData(string acct,string pass)
    {
        return DBMgr.Instance.QueryPlayerData(acct,pass);
    }

    public void AcctOnline(string acct,SeverSession severSession,PlayerData playerData)
    {
        onLineAcctDict.Add(acct, severSession);
        onLineSessionDict.Add(severSession, playerData);
    }

    public bool IsNameExist(string name)
    {
        return DBMgr.Instance.QueryNameData(name);
    }

    public PlayerData GetPlayerDataBySession(SeverSession severSession)
    {
        if(onLineSessionDict.TryGetValue(severSession,out PlayerData playerData))
        {
            return playerData;
        }
        else
        {
            return null;
        }
    }

    public bool UpdatePlayerData(int id,PlayerData playerData)
    {
        return DBMgr.Instance.UpdatePlayerData(id,playerData);
    }

    public void AcctOffLine(SeverSession severSession)
    {
        foreach(var it in onLineAcctDict)
        {
            if(it.Value==severSession)
            {
                onLineAcctDict.Remove(it.Key);
                break;
            }
        }

        onLineSessionDict.Remove(severSession);
    }
}
