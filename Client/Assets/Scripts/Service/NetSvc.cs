
using UnityEngine;
using PEProtocol;
using System.Collections.Generic;

public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance = null;

    PENet.PESocket<ClientSession, GameMsg> client = null;

    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    private static readonly string obj = "lock";

    public void InitSvc()
    {
        Instance = this;

        client = new PENet.PESocket<ClientSession, GameMsg>();
        client.SetLog(true, (string msg, int lv)=>
            {
                switch (lv)
                {
                    case 0:
                        msg = "Log:" + msg;
                        Debug.Log(msg);
                        break;
                    case 1:
                        msg = "Warn:" + msg;
                        Debug.LogWarning(msg);
                        break;
                    case 2:
                        msg = "Error:" + msg;
                        Debug.LogError(msg);
                        break;
                    case 3:
                        msg = "Info:" + msg;
                        Debug.Log(msg);
                        break;
                }

        });
        client.StartAsClient(SrvCfg.Ip, SrvCfg.port);
    }

    private void Update()
    {
       if(msgQue.Count>0)
        {
            lock(obj)
            {
                GameMsg msg = msgQue.Dequeue();
                ProcessMsg(msg);
            }
            
        }
    }

    public void SendMsg(GameMsg msg)
    {
        if(client.session!=null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("Sever not connect");
            InitSvc();
        }
    }

    public void AddNetPkg(GameMsg msg)
    {
        lock(obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    void ProcessMsg(GameMsg msg)
    {
        if(msg.err!=(int)ErrCode.None)
        {
            switch ((ErrCode)msg.err)
            {
                case ErrCode.AccIsOnline:
                    GameRoot.AddTips("当前账号以在线");
                    break;
                case ErrCode.WrongPass:
                    GameRoot.AddTips("密码错误");
                    break;
            }
            return;
        }

        switch((CMD)msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.instance.RspRename(msg);
                break;
        }
    }
}
