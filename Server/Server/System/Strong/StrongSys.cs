using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StrongSys
{
    private static StrongSys instance = null;
    public static StrongSys Instance { get { if (instance == null) instance = new StrongSys(); return instance; } }

    private CacheSvc CacheSvc;
    public void Init()
    {
        CacheSvc = CacheSvc.Instance;
        PECommon.Log("StrongSys  Init");
    }

    public void ReqStrong(MsgPack msg)
    {
        ReqStrong data = msg.GameMsg.reqStrong;

        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)CMD.RspStrong
        };

        PlayerData pd = CacheSvc.GetPlayerDataBySession(msg.SeverSession);
        int curStarLv = pd.strong[data.pos];
        StrongCfg nextSd = CfgSvc.Instance.GetStrongData(data.pos, curStarLv + 1);

        if(pd.lv<nextSd.minlv)
        {
            gameMsg.err = (int)ErrCode.LackLevel;
        }
        else if(pd.coin<nextSd.coin)
        {
            gameMsg.err = (int)ErrCode.LackCoin;
        }
        else if (pd.crystal < nextSd.crystal)
        {
            gameMsg.err = (int)ErrCode.LackCrystal;
        }
        else
        {
            pd.coin -= nextSd.coin;
            pd.crystal -= nextSd.crystal;
            pd.strong[data.pos] += 1;

            pd.hp += nextSd.addhp;
            pd.ad += nextSd.addhurt;
            pd.ap += nextSd.addhurt;
            pd.addef += nextSd.adddef;
            pd.apdef += nextSd.adddef;
        }
        if(!CacheSvc.UpdatePlayerData(pd.id,pd))
        {
            gameMsg.err = (int)ErrCode.UpdateDBError;
        }
        else
        {
            gameMsg.rspStrong = new RspStrong
            {
                coin = pd.coin,
                crystal = pd.crystal,
                hp = pd.hp,
                ad = pd.ad,
                ap = pd.ap,
                addef = pd.addef,
                apdef = pd.apdef,
                strongArr = pd.strong
            };
        }
        msg.SeverSession.SendMsg(gameMsg);
    }
}
