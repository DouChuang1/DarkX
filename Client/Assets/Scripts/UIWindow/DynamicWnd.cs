using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot {

	public Animation animation;
	public Text text;
	private bool isTipsShow = false;
	private Queue<string> tipsQue = new Queue<string>();

	public override void InitWnd()
	{
		base.InitWnd();
		SetActive(text, false);
	}

	public void AddTips(string tip)
	{
		lock(tipsQue)
		{
			tipsQue.Enqueue(tip);
		}
	}

	void Update()
	{
		if(tipsQue.Count>0 && isTipsShow==false)
		{
			lock(tipsQue)
			{
				string tip = tipsQue.Dequeue();
				isTipsShow= true;
				SetTips(tip);
			}
		}
	}
	void SetTips(string tips)
	{
		SetActive(text);
		SetText(text, tips);

		AnimationClip clip = animation.GetClip("TipsShowAni");
		animation.Play();

		StartCoroutine(AniPlayDone(clip.length, () => { SetActive(text, false); 
			isTipsShow = false;
		}));
	}

	IEnumerator AniPlayDone(float sec,Action action)
	{
		yield return new WaitForSeconds(sec);
		if(action!= null) { 
			action();
		}
	}
}
