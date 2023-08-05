using PENet;
using PEProtocol;
using System;
using System.Collections.Generic;
using System.Text;

public class MsgPack
{
    public SeverSession SeverSession;
    public GameMsg GameMsg;

    public MsgPack(SeverSession severSession,GameMsg msg)
    {
        SeverSession = severSession;
        GameMsg = msg;
    }
}

public class NetSvc
{
    private static NetSvc instance = null;
    public static NetSvc Instance { get { if (instance == null) instance = new NetSvc(); return instance; } }

    public static readonly string obj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();
    public void Init()
    {
        PESocket<SeverSession, GameMsg> server = new PESocket<SeverSession, GameMsg>();
        server.StartAsServer(SrvCfg.Ip, SrvCfg.port);
        PECommon.Log("NetSrc Init Done"); 
    }

    public void AddMsgQue(SeverSession serverSession,GameMsg msg)
    {
        lock(obj)
        {
            msgPackQue.Enqueue(new MsgPack(serverSession,msg));
        }
    }

    public void Update()
    {
        if(msgPackQue.Count>0)
        {
            lock (obj)
            {
                MsgPack pack = msgPackQue.Dequeue();
                HandleMsg(pack);
            }
        }
    }

    void HandleMsg(MsgPack msg)
    {
        switch((CMD)msg.GameMsg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(msg);
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(msg);
                break;
            case CMD.ReqGuide:
                GuideSys.Instance.ReqGuide(msg);
                break;
            case CMD.ReqStrong:
                StrongSys.Instance.ReqStrong(msg);
                break;
            case CMD.SndChat:
                ChatSys.Instance.SndChat(msg);
                break;
            case CMD.ReqBuy:
                BuySys.Instance.ReqBuy(msg);
                break;
            case CMD.ReqTakeTaskReward:
                TaskSys.Instance.ReqTaskTaskReward(msg);
                break;
            case CMD.ReqFBFight:
                FubenSys.Instance.ReqFubenFight(msg);
                break;
            case CMD.ReqFBFightEnd:
                FubenSys.Instance.ReqFubenFightEnd(msg);
                break;
        }
    }
}
