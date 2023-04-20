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
        return null;
    }

    public void AcctOnline(string acct,SeverSession severSession,PlayerData playerData)
    {
        onLineAcctDict.Add(acct, severSession);
        onLineSessionDict.Add(severSession, playerData);
    }
}
