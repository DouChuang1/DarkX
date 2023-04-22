using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoleWnd : WindowRoot {

    public InputField iptName;
    public override void InitWnd()
    {
        base.InitWnd();
        iptName.text = resSvr.GetRDNameData(false);
    }

    public void RandomName()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        iptName.text = resSvr.GetRDNameData(false);
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        if(iptName.text!="")
        {
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("名字为空");
        }
    }
}
