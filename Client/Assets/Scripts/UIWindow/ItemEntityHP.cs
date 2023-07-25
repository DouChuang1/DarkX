using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntityHP : MonoBehaviour {

    public Image imgHPGray;
    public Image imgHPRed;
    public Animation criticalAni;
    public Text txtCritical;

    public Animation dodgeAni;
    public Text txtDodge;

    public Animation hpAni;
    public Text txtHp;
    private int hpVal;
    private Transform rootTrans;
    private RectTransform rect;

    public void SetCritical(int critical)
    {
        criticalAni.Stop();
        txtCritical.text = "暴击 " + critical;
        criticalAni.Play();
    }

    public void SetDodge()
    {
        dodgeAni.Stop();
        txtDodge.text = "闪避";
        dodgeAni.Play();
    }
    public void SetHurt(int hurt)
    {
        hpAni.Stop();
        txtCritical.text = "-" + hurt;
        hpAni.Play();
    }

    public void SetItemInfo(Transform trans,int hp)
    {
        rootTrans = trans;
        rect = transform.GetComponent<RectTransform>();
        hpVal = hp;
        imgHPGray.fillAmount = 1;
        imgHPRed.fillAmount = 1;
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.position = screenPos;
        UpdateMixBlend();
        imgHPGray.fillAmount = currentPrg;
    }

    private void UpdateMixBlend()
    {
        if(Mathf.Abs(currentPrg-targetPrg)<Const.AccelerHPSpeed*Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if(currentPrg>targetPrg)
        {
            currentPrg -= Const.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg += Const.AccelerHPSpeed * Time.deltaTime;
        }
    }

    private float currentPrg;
    private float targetPrg;
    public void SetHPVal(int oldValue,int newValue)
    {
        currentPrg = oldValue * 1.0f / hpVal;
        targetPrg = newValue * 1.0f / hpVal;

        imgHPRed.fillAmount = targetPrg;

    }
}



