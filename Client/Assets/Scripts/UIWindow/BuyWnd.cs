using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWnd : WindowRoot {

    public Text txtInfo;
    private int buyType;

    public void SetBuyType(int type)
    {
        this.buyType = type;
    }

    public override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch(buyType)
        {
            case 0:
                txtInfo.text = "是否花费" + Const.Color("10钻石", TxtColor.Red) + "购买" + Const.Color("100体力", TxtColor.Green) + "?";
                break;
            case 1:
                txtInfo.text = "是否花费" + Const.Color("10钻石", TxtColor.Red) + "购买" + Const.Color("1000金币", TxtColor.Green) + "?";
                break;
        }
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                type = buyType,
                cost =10
            }
        };
        netSvc.SendMsg(msg);
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        SetWndState(false);
    }
}
