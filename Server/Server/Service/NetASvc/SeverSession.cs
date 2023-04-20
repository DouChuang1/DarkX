using System;
using PENet;
using PEProtocol;
public class SeverSession : PESession<GameMsg>
{
    protected override void OnConnected()
    {
        PECommon.Log("Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Client Req:");
        NetSvc.Instance.AddMsgQue(this,msg);
    }

    protected override void OnDisConnected()
    {
        PECommon.Log("Client Disconnect");
        
    }
}
