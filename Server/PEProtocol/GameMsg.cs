using System;
using PENet;

namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
    }

    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData PlayerData;
    }
    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
    }


    public enum ErrCode
    {
        None = 0,
        AccIsOnline,
        WrongPass,
    }

    public enum CMD
    {
        None = 0,
        ReqLogin = 101,
        RspLogin = 102,
    }

    public class SrvCfg
    {
        public const string Ip = "127.0.0.1";
        public const int port = 17666;
    }
}
