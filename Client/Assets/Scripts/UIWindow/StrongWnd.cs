using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PEProtocol;

public class StrongWnd : WindowRoot {

    public Image imgCurPos;
    public Text txtStarLv;
    public Transform starTransGrp;
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;

    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;

    public Text txtCoins;
    public Transform costTransRoot;

    public Transform posBtnParent;
    public Image[] images = new Image[6];
    private int curIndex;
    private PlayerData pd;
    StrongCfg nextSd;
    public override void InitWnd()
    {
        pd = GameRoot.Instance.PlayerData;
        base.InitWnd();
        RegClickEvts();
        ClickPosItem(0);
    }

    void RegClickEvts()
    {
        for(int i=0;i<posBtnParent.childCount;i++)
        {
            Image img = posBtnParent.GetChild(i).GetComponent<Image>();
            OnClick(img.gameObject, (object args) =>
             {
                 audioSvc.PlayUIAudio(Const.UIClickBtn);
                 ClickPosItem((int)args);
             },i);
            images[i] = img;
        }
    }

    void ClickPosItem(int index)
    {
        PECommon.Log("Click"+index);
        curIndex = index;
        for(int i=0;i<images.Length;i++)
        {
            Transform trans = images[i].transform;
            if(i==curIndex)
            {
                SetSprite(images[i], PathDefine.ItemArrowBG);
                trans.localPosition = new Vector3(10, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
            }
            else
            {
                SetSprite(images[i], PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }
        RefreshItem();
    }

    void RefreshItem()
    {
        SetText(txtCoins, pd.coin);
        switch (curIndex)
        {
            case 0:
                SetSprite(imgCurPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStarLv, pd.strong[curIndex] + "星级");

        int curtStarLv = pd.strong[curIndex];
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStarLv)
            {
                SetSprite(img, PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img, PathDefine.SpStar1);
            }
        }
        int nextStartLv = curtStarLv + 1;
        int sumAddHp = resSvr.GetPropAddValPreLv(curIndex, nextStartLv, 1);
        int sumAddHurt = resSvr.GetPropAddValPreLv(curIndex, nextStartLv, 2);
        int sumAddDef = resSvr.GetPropAddValPreLv(curIndex, nextStartLv, 3);
        SetText(propHP1, "生命 +" + sumAddHp);
        SetText(propHurt1, "伤害 +" + sumAddHurt);
        SetText(propDef1, "防御 +" + sumAddDef);


        nextSd = resSvr.GetStrongData(curIndex, nextStartLv);
        if (nextSd != null)
        {
            SetActive(propHP2);
            SetActive(propHurt2);
            SetActive(propDef2);

            SetActive(costTransRoot);
            SetActive(propArr1);
            SetActive(propArr2);
            SetActive(propArr3);

            SetText(propHP2, "强化后 +" + nextSd.addhp);
            SetText(propHurt2, "+" + nextSd.addhurt);
            SetText(propDef2, "+" + nextSd.adddef);

            SetText(txtNeedLv, "需要等级：" + nextSd.minlv);
            SetText(txtCostCoin, "需要消耗：      " + nextSd.coin);

            SetText(txtCostCrystal, nextSd.crystal + "/" + pd.crystal);
        }
        else
        {
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);

            SetActive(costTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);
        }
    }

    public void CloseStrongWnd()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        SetWndState(false);
    }

    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        if(pd.strong[curIndex]<10)
        {
            if(pd.lv<nextSd.startlv)
            {
                GameRoot.AddTips("角色等级不足");
                return;
            }
            if (pd.coin < nextSd.coin)
            {
                GameRoot.AddTips("金币数量不足");
                return;
            }
            if (pd.crystal < nextSd.crystal)
            {
                GameRoot.AddTips("水晶数量不足");
                return;
            }

            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqStrong,
                reqStrong = new ReqStrong
                {
                    pos = curIndex
                }
            });
        }
        else
        {
            GameRoot.AddTips("星级达到最大");
        }
    }

    public void UpdateUI()
    {
        audioSvc.PlayUIAudio(Const.FBEnter);
        ClickPosItem(curIndex);
    }
}
