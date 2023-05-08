using System;
using PENet;
using PEProtocol;
public class SeverSession : PESession<GameMsg>
{
    public int sessionID = 0;
    protected override void OnConnected()
    {
        sessionID = ServerRoot.Instance.GetSessionID();
        PECommon.Log("Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Client Req:");
        NetSvc.Instance.AddMsgQue(this,msg);
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log("Client Disconnect");
        
    }
}
