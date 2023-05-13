using PENet;
using System;
using System.Collections.Generic;
using System.Text;
using PEProtocol;

public class GuideSys
{
    private static GuideSys instance = null;
    public static GuideSys Instance { get { if (instance == null) instance = new GuideSys(); return instance; } }

    private CacheSvc CacheSvc;

    public void Init()
    {
        CacheSvc = CacheSvc.Instance;
        PECommon.Log("GuideSys  Init");
    }

    public void ReqGuide(MsgPack pack)
    {
        ReqGuide reqGuide = pack.GameMsg.reqGuide;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide,
        };

        PlayerData pd = CacheSvc.GetPlayerDataBySession(pack.SeverSession);

        if(pd.guideid==reqGuide.guideid)
        {
            pd.guideid += 1;
            pd.coin += CfgSvc.Instance.GetGuideCfg(reqGuide.guideid).coin;
            CalcExp(pd, CfgSvc.Instance.GetGuideCfg(reqGuide.guideid).exp);
            if(!CacheSvc.Instance.UpdatePlayerData(pd.id,pd))
            {
                msg.err = (int)ErrCode.UpdateDBError;
            }
            else
            {
                RspGuide rspGuide = new RspGuide
                {
                    guideid = pd.guideid,
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp
                };
                msg.rspGuide = rspGuide;
            }
        }
        else
        {
            msg.err = (int)ErrCode.ServerDataError;
        }

        pack.SeverSession.SendMsg(msg);
    }

    public void CalcExp(PlayerData pd,int addExp)
    {
        int curLv = pd.lv;
        int curExp = pd.exp;
        int addRestExp = addExp;
        while(true)
        {
            int upNeedUpExp = PECommon.GetExpUpValByLv(curLv) - curExp;
            if(addRestExp>=upNeedUpExp)
            {
                curLv += 1;
                curExp = 0;
                addRestExp -= upNeedUpExp;
            }
            else
            {
                pd.lv = curLv;
                pd.exp = curExp + addRestExp;
                break;
            }
        }
    }
}
