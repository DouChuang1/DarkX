using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot {
    public Image imageTouch;
    public Image imageDirBg;
    public Image imagDirPoint;

    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos;
    private float pointDis;

    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;

    public Vector2 currentDir = Vector2.zero;

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
        SetText(txtLevel, playerData.lv);
        SetText(txtName, playerData.name);
        SetExpGridByScreen();

        //显示经验条
        int expPrgVal = (int)(playerData.exp * 1.0f / PECommon.GetExpUpValByLv(playerData.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");
        int index = expPrgVal / 10;

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
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
            currentDir = Vector2.zero;
            //传递方向信息
            BattleSys.instance.SetSelfPlayerMoveDir(currentDir);
        });

        OnDrag(this.imageTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imagDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imagDirPoint.transform.position = evt.position;
            }
            currentDir = dir.normalized;
            BattleSys.instance.SetSelfPlayerMoveDir(currentDir);
            //传递方向信息
        });
    }

    public void ClickNormalAttack()
    {
        BattleSys.instance.ReqReleaseSkill(0);
    }

    public void ClickSkill1()
    {
        BattleSys.instance.ReqReleaseSkill(1);
    }
    public void ClickSkill2()
    {
        BattleSys.instance.ReqReleaseSkill(2);
    }
    public void ClickSkill3()
    {
        BattleSys.instance.ReqReleaseSkill(3);
    }

}
