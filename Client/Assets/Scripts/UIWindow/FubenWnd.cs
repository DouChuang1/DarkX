using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FubenWnd : WindowRoot {

    public Transform[] fbBtnArr;
    public Transform pointerTrans;
    private PlayerData pd;
    public override void InitWnd()
    {
        pd = GameRoot.Instance.PlayerData;
        base.InitWnd();
        RefreshUI();
    }

    public void RefreshUI()
    {
        int fbId = pd.fuben;
        for(int i=0;i<fbBtnArr.Length;i++)
        {
            if(i<fbId%10000)
            {
                SetActive(fbBtnArr[i].gameObject);
                if(i==fbId%10000-1)
                {
                    pointerTrans.SetParent(fbBtnArr[i].transform);
                    pointerTrans.localPosition = new Vector3(25, 100, 0);
                }
            }
            else
            {
                SetActive(fbBtnArr[i].gameObject, false);
            }
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        SetWndState(false);
    }

    public void ClickTaskBtn(int fid)
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        int needPower = resSvr.GetMapCfgData(fid).power;
        if(needPower>pd.power)
        {
            GameRoot.AddTips("体力不足");
        }
        else
        {
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqFBFight,
                reqFBFight = new ReqFBFight
                {
                    fbid = fid
                }
            };

            netSvc.SendMsg(msg);
        }
    }
}
