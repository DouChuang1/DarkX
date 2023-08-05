using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FBEndType
{
    None,
    Pause,
    Win,
    Lose
}

public class BattleEndWnd : WindowRoot {
    public Transform rewardTrans;
    public Button btnClose;
    public Button btnExit;
    public Button btnSure;

    public Text txtTime;
    public Text txtRestHP;
    public Text txtReward;
    public Animation ani;
    private FBEndType endType = FBEndType.None;
    public override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    public void SewtWndType(FBEndType endType)
    {
        this.endType = endType;
    }

    private void RefreshUI()
    {
        switch(endType)
        {
            case FBEndType.Pause:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject);
                break;
            case FBEndType.Win:
                SetActive(rewardTrans, true);
                SetActive(btnExit.gameObject,false);
                SetActive(btnClose.gameObject,false);
                MapCfg cfg = resSvr.GetMapCfgData(fbid);
                int min = costtime / 60;
                int sec = costtime % 60;
                int coin = cfg.coin;
                int exp = cfg.exp;
                int crystal = cfg.crystal;
                SetText(txtTime, "通关时间：" + min + ":" + sec);
                SetText(txtRestHP, "剩余血量：" + resthp);
                SetText(txtReward, "关卡奖励：" + Const.Color(coin + "金币", TxtColor.Green) + Const.Color(exp + "经验", TxtColor.Yellow) + Const.Color(crystal + "水晶", TxtColor.Blue));

                TimerSvc.Instance.AddTimeTask((int tid) => {
                    SetActive(rewardTrans);
                    ani.Play();
                    TimerSvc.Instance.AddTimeTask((int tid1) => {
                        audioSvc.PlayUIAudio(Const.FBItemEnter);
                        TimerSvc.Instance.AddTimeTask((int tid2) => {
                            audioSvc.PlayUIAudio(Const.FBItemEnter);
                            TimerSvc.Instance.AddTimeTask((int tid3) => {
                                audioSvc.PlayUIAudio(Const.FBItemEnter);
                                TimerSvc.Instance.AddTimeTask((int tid5) => {
                                    audioSvc.PlayUIAudio(Const.FBLogoEnter);
                                }, 300);
                            }, 270);
                        }, 270);
                    }, 325);
                }, 1000);
                break;
            case FBEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject,false);
                break;
        }
    }

    private int fbid;
    private int costtime;
    private int resthp;
    public void SetBattleEndData(int fbid,int costtime,int restHp)
    {
        this.fbid = fbid;
        this.costtime = costtime;
        resthp = restHp;
    }

    public void ClickClose()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        BattleSys.instance.battleMgr.isPauseGame = false;
        SetWndState(false);
    }

    public void ClickExit()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
       
        BattleSys.instance.DistroyBattle();
        MainCitySys.instance.EnterMainCity();
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        BattleSys.instance.DistroyBattle();
        MainCitySys.instance.EnterMainCity();
        FubenSys.instance.EnterFuben();
    }
}
