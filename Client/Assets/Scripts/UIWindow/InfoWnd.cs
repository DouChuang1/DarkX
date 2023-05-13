using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot {

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHp;
    public Text txtHurt;
    public Text txtDef;

    public RawImage charImg;
    private Vector2 startPos;

    public Transform transDetail;
    public Text dxthp;
    public Text dxtad;
    public Text dxtap;
    public Text dxtaddef;
    public Text dxtapdef;
    public Text dxtdodge;
    public Text dxtpierce;
    public Text dxtcritical;


    public override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvts();
        RefreshUI();
    }

    private void RegTouchEvts()
    {
        OnClickDown(charImg.gameObject, (PointerEventData evt) =>
         {
             startPos = evt.position;
             MainCitySys.instance.SetStartRotate();
         });

        OnDrag(charImg.gameObject, (PointerEventData evt) =>
        {
            float rotate = -(evt.position.x - startPos.x)*0.4f;
            MainCitySys.instance.SetPlayerRotate(rotate);
        });
    }

    void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtInfo, pd.name + " LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv);

        SetText(txtFight, pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);

        SetText(txtJob, "职业 暗夜刺客");
        SetText(txtFight, "战力  " + PECommon.GetFightByProps(pd));
        SetText(txtHp, "血量  " + pd.hp);
        SetText(txtHurt, "伤害  " + (pd.ad + pd.ap));
        SetText(txtDef, "防御  " + (pd.addef + pd.apdef));

        SetText(dxthp,pd.hp);
        SetText(dxtad, pd.ad);
        SetText(dxtap, pd.ap);
        SetText(dxtaddef, pd.addef);
        SetText(dxtapdef, pd.apdef);
        SetText(dxtdodge, pd.dodge+"%");
        SetText(dxtpierce, pd.pierce+"%");
        SetText(dxtcritical, pd.critical+"%");
    }

    public void OnClickClose()
    {
        AudioSvc.Instance.PlayUIAudio(Const.UIClickBtn);
        MainCitySys.instance.CloseInfoWnd();
    }

    public void OnClickOpenDetailInfo()
    {
        AudioSvc.Instance.PlayUIAudio(Const.UIClickBtn);
        SetActive(transDetail);
    }

    public void OnClickCloseDetailInfo()
    {
        AudioSvc.Instance.PlayUIAudio(Const.UIClickBtn);
        SetActive(transDetail,false);
    }
}
