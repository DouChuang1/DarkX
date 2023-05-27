using PENet;
using System;
using System.Collections.Generic;
using System.Text;
using PEProtocol;

public class LoginSys
{
    private static LoginSys instance = null;
    public static LoginSys Instance { get { if (instance == null) instance = new LoginSys(); return instance; } }

    public void Init()
    {
        PECommon.Log("Login Sys Init");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin reqLogin = pack.GameMsg.reqLogin;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
        };

        if(CacheSvc.Instance.IsAcctOnline(reqLogin.acct))
        {
            msg.err = (int)ErrCode.AccIsOnline;
        }
        else
        {
            PlayerData playerData = CacheSvc.Instance.GetPlayerData(reqLogin.acct, reqLogin.pass);
            if(playerData==null)
            {
                msg.err = (int)ErrCode.WrongPass;
            }
            else
            {
                int power = playerData.power;
                long now = TimerSvc.Instance.GetNow();
                long milliseconds = now - playerData.time;
                int addPower = (int)(milliseconds / (1000 * 60 * PECommon.PowerAddSpace) * PECommon.PowerAddCount);
                if(addPower>0)
                {
                    int powMax = PECommon.GetPowerLimit(playerData.lv);
                    if(playerData.power<powMax)
                    {
                        playerData.power += addPower;
                        if(playerData.power>powMax)
                        {
                            playerData.power = powMax;
                        }
                    }
                }
                if(power!=playerData.power)
                {
                    CacheSvc.Instance.UpdatePlayerData(playerData.id, playerData);
                }
                msg.rspLogin = new RspLogin { PlayerData = playerData};
                CacheSvc.Instance.AcctOnline(reqLogin.acct, pack.SeverSession, playerData);
            }
        }
        pack.SeverSession.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.GameMsg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename,
        };
        if(CacheSvc.Instance.IsNameExist(data.name))
        {
            msg.err = (int)ErrCode.NameExist;
        }
       else
        {
            PlayerData playerData = CacheSvc.Instance.GetPlayerDataBySession(pack.SeverSession);
            playerData.name = data.name;
            if(!CacheSvc.Instance.UpdatePlayerData(playerData.id,playerData))
            {
                msg.err = (int)ErrCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
            }
        }
        pack.SeverSession.SendMsg(msg);
    }

    public void ClearOfflineData(SeverSession severSession)
    {
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(severSession);
        if(pd!=null)
        {
            pd.time = TimerSvc.Instance.GetNow();
            if(!CacheSvc.Instance.UpdatePlayerData(pd.id,pd))
            {

            }
        }
        CacheSvc.Instance.AcctOffLine(severSession);
    }
}
