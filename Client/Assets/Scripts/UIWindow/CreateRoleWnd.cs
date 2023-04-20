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
}
