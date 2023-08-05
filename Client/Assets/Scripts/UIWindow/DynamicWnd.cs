using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot {

	public Animation animation;
    public Animation selfDodgeAni;
	public Text text;
    public Transform hpItemRoot;
	private bool isTipsShow = false;
	private Queue<string> tipsQue = new Queue<string>();
    private Dictionary<string, ItemEntityHP> itemDic = new Dictionary<string, ItemEntityHP>();
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

    public void AddHpItemInfo(string mName,Transform trans,int hp)
    {
        ItemEntityHP item = null;
        if(itemDic.TryGetValue(mName, out item))
        {
            return;
        }
        else
        {
            GameObject go = resSvr.LoadPrefab(PathDefine.HPItemPrefab, true);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = new Vector3(-1000, 0, 0);
            ItemEntityHP ieh = go.GetComponent<ItemEntityHP>();
            ieh.SetItemInfo(trans,hp);
            itemDic.Add(mName, ieh);
        }
    }
    public void RmvHpItemInfo(string mName)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(mName, out item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(mName);
        }
    }


    public void SetDodge(string key)
    {
        ItemEntityHP item = null;
        if(itemDic.TryGetValue(key,out item))
        {
            item.SetDodge();
        }
    }
    public void SetCritical(string key,int critical)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetCritical(critical);
        }
    }

    public void SetHurt(string key,int hurt)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHurt(hurt);
        }
    }

    public void SetHpValue(string key, int oldValue,int newValue)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHPVal(oldValue,newValue);
        }
    }

    public void SetSelfDodge()
    {
        selfDodgeAni.Stop();
        selfDodgeAni.Play();
    }

    public void RmvAllHPItemInfo()
    {
        foreach(var item in itemDic)
        {
            Destroy(item.Value.gameObject);
        }
        itemDic.Clear();
    }
}
