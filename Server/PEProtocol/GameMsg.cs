using System;
using PENet;

namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqRename reqRename;
        public RspRename rspRename;
        public ReqGuide reqGuide;
        public RspGuide rspGuide;
        public ReqStrong reqStrong;
        public RspStrong rspStrong;
        public SndChat sndChat;
        public PshChat pshChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;
        public PshPower pshPower;

        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTakeTaskReward rspTakeTaskReward;
        public PshTaskPrgs pshTaskPrgs;

        public ReqFBFight reqFBFight;
        public RspFBFight rspFBFight;

        public ReqFBFightEnd reqFBFightEnd;
        public RspFBFightEnd rspFBFightEnd;
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
        public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;
        public int pierce;
        public int critical;
        public int guideid;
        public int[] strong;
        public long time;
        public string[] taskArr;
        public int fuben;
    }
    [Serializable]
    public class ReqRename
    {
        public string name;
    }

    [Serializable]
    public class RspRename
    {
        public string name;
    }

    [Serializable]
    public class ReqGuide
    {
        public int guideid;
    }

    [Serializable]
    public class RspGuide
    {
        public int guideid;
        public int coin;
        public int lv;
        public int exp;
    }

    [Serializable]
    public class ReqStrong
    {
        public int pos;
    }

    [Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }

    [Serializable]
    public class SndChat
    {
        public string chat;
    }
    [Serializable]
    public class PshChat
    {
        public string name;
        public string chat;
    }
    [Serializable]
    public class PshPower
    {
        public int power;
    }

    [Serializable]
    public class ReqBuy
    {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuy
    {
        public int type;
        public int diamond;
        public int coin;
        public int power;
    }

    [Serializable]
    public class ReqFBFight
    {
        public int fbid;
    }

    [Serializable]
    public class RspFBFight
    {
        public int fbid;
        public int power;
    }

    [Serializable]
    public class ReqFBFightEnd
    {
        public bool win;
        public int fbid;
        public int resthp;
        public int costtime;
    }

    [Serializable]
    public class RspFBFightEnd
    {
        public bool win;
        public int fbid;
        public int resthp;
        public int costtime;

        public int coin;
        public int lv;
        public int crystal;
        public int exp;
        public int fuben;
    }

    [Serializable]
    public class ReqTakeTaskReward
    {
        public int rid;
    }

    [Serializable]
    public class RspTakeTaskReward
    {
        public int coin;
        public int lv;
        public int exp;
        public string[] taskArr;
    }

    [Serializable]
    public class PshTaskPrgs
    {
        public string[] taskArr;
    }

    public enum ErrCode
    {
        None = 0,
        AccIsOnline,
        WrongPass,
        NameExist,
        UpdateDBError,
        ServerDataError,
        LackLevel,
        LackCoin,
        LackCrystal,
        LackDiamond,
        ClientDataError,
        LackPower,
    }

    public enum CMD
    {
        None = 0,
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename=103,
        RspRename = 104,
        ReqGuide = 200,
        RspGuide = 201,
        ReqStrong = 202,
        RspStrong = 203,

        SndChat = 204,
        pshChat = 205,

        ReqBuy = 206,
        RspBuy = 207,

        PshPower = 208,

        ReqTakeTaskReward = 209,
        RspTakeTaskReward = 210,
        PshTaskPrgs = 211,
        ReqFBFight = 301,
        RspFBFight = 302,

        ReqFBFightEnd = 303,
        RspFBFightEnd = 304,
    }

    public class SrvCfg
    {
        public const string Ip = "127.0.0.1";
        public const int port = 17666;
    }
}
