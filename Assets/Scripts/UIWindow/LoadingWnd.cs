using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot {

	public Text txtTips;
	public Image imgFG;
	public Image imgPoint;
	public Text txtPrg;

	private float fgWidth;

	public override void InitWnd()
	{
		base.InitWnd();
		fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
		txtTips.text = "这是一条游戏tips";
		txtPrg.text = "0%";
		imgFG.fillAmount= 0;
		imgPoint.transform.localPosition = new Vector3(-545, 0, 0);

	}

	public void SetProgress(float prg)
	{
		txtPrg.text = (int)(prg * 100) + "%";
		imgFG.fillAmount = prg;
        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
        float posX = prg * fgWidth - 545;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
	}
}
