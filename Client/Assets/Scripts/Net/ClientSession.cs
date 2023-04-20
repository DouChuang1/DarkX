
using PEProtocol;
using UnityEngine;
public class ClientSession : PENet.PESession<GameMsg>
{
    protected override void OnConnected()
    {
        Debug.Log("Server Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Debug.Log("Server Rsp:"+((CMD)(msg.cmd)).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }

    protected override void OnDisConnected()
    {
        Debug.Log("Server DisConnect");
    }
}
