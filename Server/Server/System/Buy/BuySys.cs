using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuySys
{
    private static BuySys instance = null;
    public static BuySys Instance { get { if (instance == null) instance = new BuySys(); return instance; } }

    public void Init()
    {
        PECommon.Log("BuySys  Init");
    }

    internal void ReqBuy(MsgPack pack)
    {
        ReqBuy data = pack.GameMsg.reqBuy;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspBuy
        };

        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.SeverSession);
        if(pd.diamond<data.cost)
        {
            msg.err = (int)ErrCode.LackDiamond;
        }
        else
        {
            pd.diamond -= data.cost;
            switch(data.type)
            {
                case 0:
                    pd.power += 100;
                    break;
                case 1:
                    pd.coin += 1000;
                    break;
            }
            if(!CacheSvc.Instance.UpdatePlayerData(pd.id,pd))
            {
                msg.err = (int)ErrCode.UpdateDBError;
            }
            else
            {
                RspBuy rspBuy = new RspBuy
                {
                    type = data.type,
                    diamond = pd.diamond,
                    coin = pd.coin,
                    power = pd.power
                };
                msg.rspBuy = rspBuy;
            }
        }
        pack.SeverSession.SendMsg(msg);
    }
}
