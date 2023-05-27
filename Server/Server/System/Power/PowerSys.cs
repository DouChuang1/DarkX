using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PowerSys
{
    private static PowerSys instance = null;
    public static PowerSys Instance { get { if (instance == null) instance = new PowerSys(); return instance; } }

    private CacheSvc CacheSvc;
    public void Init()
    {
        CacheSvc = CacheSvc.Instance;
        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Second, 0);
        PECommon.Log("PowerSys  Init");
    }

    private void CalcPowerAdd(int tid)
    {
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshPower
        };
        msg.pshPower = new PshPower();
        Dictionary<SeverSession, PlayerData> onlineDic = CacheSvc.onLineSessionDict;
        foreach(var item in onlineDic)
        {
            PlayerData pd = item.Value;
            SeverSession session = item.Key;

            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if(pd.power>=powerMax)
            {
                continue;
            }
            else
            {
                pd.power += PECommon.PowerAddCount;
                pd.time = TimerSvc.Instance.GetNow();
                if(pd.power>powerMax)
                {
                    pd.power = powerMax;
                }
            }
            if(!CacheSvc.UpdatePlayerData(pd.id,pd))
            {
                msg.err = (int)ErrCode.UpdateDBError;
            }
            else
            {
                msg.pshPower.power = pd.power;
                session.SendMsg(msg);
            }
        }

    }

}
