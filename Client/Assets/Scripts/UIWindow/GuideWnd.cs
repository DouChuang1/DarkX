using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot {

    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;
    private AutoGuideCfg curTaskData;
    private string[] dialogArr;
    private int index;
    private PlayerData pd;
    public override void InitWnd()
    {
        base.InitWnd();
        curTaskData = MainCitySys.instance.GetCurTaskData();
        pd = GameRoot.Instance.PlayerData;
        dialogArr = curTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
    }

    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        if(talkArr[0]=="0")
        {
            SetSprite(imgIcon, PathDefine.SelfIcon);
            SetText(txtName, pd.name);
        }
        else
        {
            switch(curTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小云");
                    break;

            }
        }
        imgIcon.SetNativeSize();
        SetText(txtTalk, talkArr[1].Replace("$name", pd.name));
    }

    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        index += 1;
        if(index==dialogArr.Length)
        {
            GameMsg gameMsg = new GameMsg
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide
                {
                    guideid = curTaskData.ID
                }
            };
            netSvc.SendMsg(gameMsg);
            SetWndState(false);
        }
        else
        {
            SetTalk();
        }
    }
}
