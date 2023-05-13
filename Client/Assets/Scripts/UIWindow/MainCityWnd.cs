using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWnd : WindowRoot {

    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;
    public Animation menuAni;

    public Image imageTouch;
    public Image imageDirBg;
    public Image imagDirPoint;

    private bool menuState = true;

    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos;
    private float pointDis;

    private AutoGuideCfg AutoGuideCfg;
    public Button buttonGuide;
    public override void InitWnd()
    {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Const.ScreenPresetHeight * Const.ScreenOpDis;
        defaultPos = imageDirBg.transform.position;
        SetActive(imagDirPoint, false);
        RegisterTouchEvts();
        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData playerData = GameRoot.Instance.PlayerData;
        SetText(txtFight, PECommon.GetFightByProps(playerData));
        SetText(txtPower, "体力:"+playerData.power+"/"+PECommon.GetPowerLimit(playerData.lv));
        imgPowerPrg.fillAmount = playerData.power * 1.0f / PECommon.GetPowerLimit(playerData.lv);
        SetText(txtLevel, playerData.lv);
        SetText(txtName, playerData.name);
        SetExpGridByScreen();

        //显示经验条
        int expPrgVal = (int)(playerData.exp * 1.0f / PECommon.GetExpUpValByLv(playerData.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");
        int index = expPrgVal / 10;

        for(int i=0;i<expPrgTrans.childCount;i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if(i<index)
            {
                img.fillAmount = 1;
            }
            else if(i==index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }

        AutoGuideCfg = resSvr.GetAutoGuideData(playerData.guideid);
        if(AutoGuideCfg!=null)
        {
            SetGuideBtnIcon(AutoGuideCfg.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }
    }

    void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image Img = buttonGuide.GetComponent<Image>();

        switch(npcID)
        {
            case Const.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Const.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Const.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Const.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(Img, spPath);
    }

    public void ClickGuideBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Const.UIClickBtn);
        if(AutoGuideCfg!=null)
        {
            MainCitySys.instance.RunTask(AutoGuideCfg);
        }
        else
        {
            GameRoot.AddTips("引导结束");
        }
    }

    private void SetExpGridByScreen()
    {
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();

        float globalRate = 1.0f * Const.ScreenPresetHeight / Screen.height; //根据自适应 是基于高度来计算
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);
    }

    private void Update()
    {
        SetExpGridByScreen();
    }

    public void ClickMenuBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Const.UIExtenBtn);
        menuState = !menuState;
        AnimationClip clip = null;
        if(menuState)
        {
            clip = menuAni.GetClip("OpenMCMenu");
        }
        else
        {
            clip = menuAni.GetClip("CloseMCMenu");
        }
        menuAni.Play(clip.name);
    }

    public void ClickHeadBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Const.UIOpenBtn);
        MainCitySys.instance.OpenInfoWnd();
    }

    public void RegisterTouchEvts()
    {
        OnClickDown(this.imageTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            SetActive(imagDirPoint);
            imageDirBg.transform.position = evt.position;
        });

        OnClickUp(this.imageTouch.gameObject, (PointerEventData evt) =>
        {
            imageDirBg.transform.position = defaultPos;
            SetActive(imagDirPoint, false);
            imagDirPoint.transform.localPosition = Vector2.zero;

            //传递方向信息
            MainCitySys.instance.SetMoveDir(Vector2.zero);
        });

        OnDrag(this.imageTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if(len> pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imagDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imagDirPoint.transform.position = evt.position;
            }
            MainCitySys.instance.SetMoveDir(dir.normalized);
            //传递方向信息
        });
    }
}
