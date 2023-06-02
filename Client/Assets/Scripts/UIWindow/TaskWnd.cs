using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskWnd : WindowRoot {
    public Transform parent;
    private PlayerData pd;
    private List<TaskRewardData> trdList = new List<TaskRewardData>();
    public override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        trdList.Clear();
        List<TaskRewardData> todoList = new List<TaskRewardData>();
        List<TaskRewardData> doneList = new List<TaskRewardData>();
        for(int i=0;i<pd.taskArr.Length;i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1")
            };

            if(trd.taked)
            {
                doneList.Add(trd);
            }
            else
            {
                todoList.Add(trd);
            }
        }
        trdList.AddRange(todoList);
        trdList.AddRange(doneList);
        for (int i=0;i<parent.childCount;i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        for(int i=0;i<trdList.Count;i++)
        {
            GameObject go = resSvr.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(parent);
            go.name = "taskItem_" + i;

            TaskRewardData trd = trdList[i];
            TaskRewardCfg trc = resSvr.GetTaskRewardData(trd.ID);
            SetText(GetTrans(go.transform, "txtName"), trc.taskName);
            SetText(GetTrans(go.transform, "txtPrg"), trd.prgs + "/" + trc.count);
            SetText(GetTrans(go.transform, "txtExp"), "奖励：    经验" + trc.exp);
            SetText(GetTrans(go.transform, "txtCoin"), "金币" + trc.coin);
            Image imgPrg = GetTrans(go.transform, "prgBar/prgVal").GetComponent<Image>();
            float prgVal = trd.prgs * 1.0f / trc.count;
            imgPrg.fillAmount = prgVal;

            Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
            btnTake.onClick.AddListener(() =>
            {
                ClickTaskBtn(go.name);
            });

            Transform transComp = GetTrans(go.transform, "imgComp");
            if(trd.taked)
            {
                btnTake.interactable = false;
                SetActive(transComp);
            }
            else
            {
                SetActive(transComp, false);
                if (trd.prgs == trc.count)
                {
                    btnTake.interactable = true;
                }
                else
                {
                    btnTake.interactable = false;
                }
                  
            }
        }
    }
    public void CloseTaskWnd()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        SetWndState(false);
    }

    public void ClickTaskBtn(string name)
    {
        string[] nameArr = name.Split('_');
        int index = int.Parse(nameArr[1]);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqTakeTaskReward,
            reqTakeTaskReward = new ReqTakeTaskReward
            {
                rid = trdList[index].ID
            }
        };

        netSvc.SendMsg(msg);

    }

}
