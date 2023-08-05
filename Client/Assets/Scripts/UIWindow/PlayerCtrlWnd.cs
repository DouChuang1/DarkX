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

        int hp = GameRoot.Instance.PlayerData.hp;
        SetText(txtSelfHP, hp + "/" + hp);
        imgSelfHP.fillAmount = 1;
        SetBossHPBarState(false);
        RegisterTouchEvts();
        sk1CDTime = resSvr.GetSkillCfg(101).cdTime/1000;
        sk2CDTime = resSvr.GetSkillCfg(102).cdTime / 1000;
        sk3CDTime = resSvr.GetSkillCfg(103).cdTime / 1000;
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

    public Image imgSk1CD;
    public Text txtSk1CD;
    private bool isSk1CD = false;
    private float sk1CDTime;

    private int sk1Num;
    private float sk1FillCount;
    private float sk1NumCount;

    public Image imgSk2CD;
    public Text txtSk2CD;
    private bool isSk2CD = false;
    private float sk2CDTime;

    private int sk2Num;
    private float sk2FillCount;
    private float sk2NumCount;

    public Image imgSk3CD;
    public Text txtSk3CD;
    private bool isSk3CD = false;
    private float sk3CDTime;

    private int sk3Num;
    private float sk3FillCount;
    private float sk3NumCount;


    public Text txtSelfHP;
    public Image imgSelfHP;
    private void Update()
    {
        float delta = Time.deltaTime;
        if(isSk1CD)
        {
            sk1FillCount+= delta;
            if(sk1FillCount>=sk1CDTime)
            {
                isSk1CD = false;
                sk1FillCount = 0;
                SetActive(imgSk1CD, false);
            }
            else
            {
                imgSk1CD.fillAmount = 1 - sk1FillCount / sk1CDTime;
            }

            sk1NumCount += delta;
            if(sk1NumCount>=1)
            {
                sk1NumCount -= 1;
                sk1Num -= 1;
                SetText(txtSk1CD, sk1Num);
            }
        }

        if (isSk2CD)
        {
            sk2FillCount += delta;
            if (sk2FillCount >= sk2CDTime)
            {
                isSk2CD = false;
                sk2FillCount = 0;
                SetActive(imgSk2CD, false);
            }
            else
            {
                imgSk2CD.fillAmount = 1 - sk2FillCount / sk2CDTime;
            }

            sk2NumCount += delta;
            if (sk2NumCount >= 1)
            {
                sk2NumCount -= 1;
                sk2Num -= 1;
                SetText(txtSk2CD, sk2Num);
            }
        }


        if (isSk3CD)
        {
            sk3FillCount += delta;
            if (sk3FillCount >= sk3CDTime)
            {
                isSk3CD = false;
                sk3FillCount = 0;
                SetActive(imgSk3CD, false);
            }
            else
            {
                imgSk3CD.fillAmount = 1 - sk3FillCount / sk3CDTime;
            }

            sk3NumCount += delta;
            if (sk3NumCount >= 1)
            {
                sk3NumCount -= 1;
                sk3Num -= 1;
                SetText(txtSk3CD, sk3Num);
            }
        }

        if(transBossHPBar.gameObject.activeSelf)
        {
            BlendBossHP();
            imgYellow.fillAmount = currentPrg;
        }
    }

    public void ClickSkill1()
    {
        if(isSk1CD==false && GetCanRlsSkill())
        {
            BattleSys.instance.ReqReleaseSkill(1);
            isSk1CD = true;
            SetActive(imgSk1CD);
            imgSk1CD.fillAmount = 1;
            sk1Num = (int)sk1CDTime;
            SetText(txtSk1CD, sk1Num);
        }
        
    }
    public void ClickSkill2()
    {
        if (isSk2CD == false && GetCanRlsSkill())
        {
            BattleSys.instance.ReqReleaseSkill(2);
            isSk2CD = true;
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            sk2Num = (int)sk2CDTime;
            SetText(txtSk2CD, sk2Num);
        }
    }
    public void ClickSkill3()
    {
        if (isSk3CD == false && GetCanRlsSkill())
        {
            BattleSys.instance.ReqReleaseSkill(3);
            isSk3CD = true;
            SetActive(imgSk3CD);
            imgSk3CD.fillAmount = 1;
            sk3Num = (int)sk3CDTime;
            SetText(txtSk3CD, sk3Num);
        }
    }

    public void SetSelfHP(int val)
    {
        SetText(txtSelfHP, val + "/" + GameRoot.Instance.PlayerData.hp);
        imgSelfHP.fillAmount = val * 1.0f / GameRoot.Instance.PlayerData.hp;
    }

    public bool GetCanRlsSkill()
    {
        return BattleSys.instance.battleMgr.CanRlsSkill();
    }

    public Transform transBossHPBar;
    public Image imgRed;
    public Image imgYellow;

    private float currentPrg = 1;
    private float targetPrg = 1;

    public void SetBossHPBarState(bool state,float prg=1)
    {
        SetActive(transBossHPBar, state);
        imgRed.fillAmount = 1;
        imgYellow.fillAmount = 1;
    }

    public void SetBossHPBarVal(int oldVal,int newVal,int sumVal)
    {
        currentPrg = oldVal * 1.0f / sumVal;
        targetPrg = newVal * 1.0f / sumVal;

        imgRed.fillAmount = targetPrg;
    }

    private void BlendBossHP()
    {
        if (Mathf.Abs(currentPrg - targetPrg) < Const.AccelerHPSpeed * Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if (currentPrg > targetPrg)
        {
            currentPrg -= Const.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg += Const.AccelerHPSpeed * Time.deltaTime;
        }
    }

    public void ClickHeadBtn()
    {
        BattleSys.instance.battleMgr.isPauseGame = true;
        BattleSys.instance.SetBattleEndWndState(FBEndType.Pause, true);
    }
}
