using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FubenSys
{
    private static FubenSys instance = null;
    public static FubenSys Instance { get { if (instance == null) instance = new FubenSys(); return instance; } }

    public void Init()
    {
        PECommon.Log("Fuben Sys Init");
    }

    public void ReqFubenFight(MsgPack pack)
    {
        ReqFBFight data = pack.GameMsg.reqFBFight;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFight,
        };

        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.SeverSession);
        int needPower = CfgSvc.Instance.GetMapCfgData(data.fbid).power;
        if(pd.fuben<data.fbid)
        {
            msg.err = (int)ErrCode.ClientDataError;
        }
        else if(pd.power<needPower)
        {
            msg.err = (int)ErrCode.LackPower;
        }
        else
        {
            pd.power -= needPower;
            if(CacheSvc.Instance.UpdatePlayerData(pd.id,pd))
            {
                RspFBFight rspFBFight = new RspFBFight
                {
                    fbid = data.fbid,
                    power = pd.power
                };
                msg.rspFBFight = rspFBFight;
            }
            else
            {
                msg.err = (int)ErrCode.UpdateDBError;
            }
        }
        pack.SeverSession.SendMsg(msg);
    }

    public void ReqFubenFightEnd(MsgPack pack)
    {
        ReqFBFightEnd data = pack.GameMsg.reqFBFightEnd;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFightEnd
        };

        if(data.win)
        {
            if(data.costtime>0 && data.resthp>0)
            {
                MapCfg rd = CfgSvc.Instance.GetMapCfgData(data.fbid);
                PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.SeverSession);

                TaskSys.Instance.CalcTaskPrgs(pd, 2);
                pd.coin += rd.coin;
                pd.crystal += rd.crystal;
                PECommon.CalcExp(pd, rd.exp);

                if(pd.fuben==data.fbid)
                {
                    pd.fuben += 1;
                }

                if(!CacheSvc.Instance.UpdatePlayerData(pd.id,pd))
                {
                    msg.err = (int)ErrCode.UpdateDBError;
                }
                else
                {
                    RspFBFightEnd rspFBFightEnd = new RspFBFightEnd
                    {
                        win = data.win,
                        fbid = data.fbid,
                        costtime = data.costtime,
                        resthp = data.resthp,
                        coin = pd.coin,
                        lv = pd.lv,
                        exp = pd.exp,
                        crystal = pd.crystal,
                        fuben = pd.fuben
                    };
                    msg.rspFBFightEnd = rspFBFightEnd;
                }
            }
        }
        else
        {
            msg.err = (int)ErrCode.ClientDataError;
        }

        pack.SeverSession.SendMsg(msg);
    }
}
