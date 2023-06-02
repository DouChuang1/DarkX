using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TaskSys
{
    private static TaskSys instance = null;
    public static TaskSys Instance { get { if (instance == null) instance = new TaskSys(); return instance; } }

    private CacheSvc CacheSvc;
    public void Init()
    {
        CacheSvc = CacheSvc.Instance;
        PECommon.Log("TaskSys  Init");
    }

    public void ReqTaskTaskReward(MsgPack pack)
    {
        ReqTakeTaskReward data = pack.GameMsg.reqTakeTaskReward;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspTakeTaskReward
        };

        PlayerData pd = CacheSvc.GetPlayerDataBySession(pack.SeverSession);

        TaskRewardCfg trc = CfgSvc.Instance.GetTaskRewardCfg(data.rid);

        TaskRewardData trd = CalcTaskRewardData(pd, data.rid);
        if(trd.prgs==trc.count && !trd.taked)
        {
            pd.coin += trc.coin;
            PECommon.CalcExp(pd, trc.exp);
            trd.taked = true;
            CalcTaskArr(pd, trd);
            if(!CacheSvc.UpdatePlayerData(pd.id,pd))
            {
                msg.err = (int)ErrCode.UpdateDBError;
            }
            else
            {
                RspTakeTaskReward rspTakeTaskReward = new RspTakeTaskReward
                {
                    coin = pd.coin,
                    exp = pd.exp,
                    lv = pd.lv,
                    taskArr = pd.taskArr
                };
                msg.rspTakeTaskReward = rspTakeTaskReward;
            }
        }
        else
        {
            msg.err = (int)ErrCode.ClientDataError;
        }
        pack.SeverSession.SendMsg(msg);
    }

    private TaskRewardData CalcTaskRewardData(PlayerData pd,int rid)
    {
        TaskRewardData trd = null;
        for(int i=0;i<pd.taskArr.Length;i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if(int.Parse(taskInfo[0])==rid)
            {
                trd = new TaskRewardData
                {
                    ID = rid,
                    prgs = int.Parse(taskInfo[1]),
                    taked = taskInfo[2].Equals("1")
                };
                break;
            }
        }
        return trd;
    }

    public void CalcTaskArr(PlayerData pd,TaskRewardData trd)
    {
        string result = trd.ID + "|" + trd.prgs + "|" + (trd.taked ? 1 : 0);
        int index = -1;
        for(int i=0;i<pd.taskArr.Length;i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if(int.Parse(taskInfo[0])==trd.ID)
            {
                index = i;
                break;
            }
        }
        pd.taskArr[index] = result;
    }

    public void CalcTaskPrgs(PlayerData pd,int tid)
    {
        TaskRewardData trd = CalcTaskRewardData(pd, tid);
        TaskRewardCfg trc = CfgSvc.Instance.GetTaskRewardCfg(tid);

        if(trd.prgs<trc.count)
        {
            trd.prgs += 1;
            CalcTaskArr(pd, trd);

            SeverSession severSession = CacheSvc.GetOnLineServerSession(pd.id);
            if(severSession!=null)
            {
                severSession.SendMsg(new GameMsg
                {
                    cmd = (int)CMD.PshTaskPrgs,
                    pshTaskPrgs = new PshTaskPrgs
                    {
                        taskArr = pd.taskArr
                    }
                });
            }
        }
    }

}
