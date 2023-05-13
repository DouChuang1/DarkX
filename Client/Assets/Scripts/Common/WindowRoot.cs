using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour {

	public ResSvr resSvr;
	public AudioSvc audioSvc;
    protected NetSvc netSvc;
	public void SetWndState(bool isActive = true)
	{
		if(gameObject.activeSelf!=isActive)
		{
			gameObject.SetActive(isActive);
		}
		if(isActive)
		{
			InitWnd();
		}
		else
		{
			ClearWnd();
		}
	}

	public virtual void InitWnd()
	{
		resSvr = ResSvr.instance;
		audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
	}

	public void ClearWnd()
	{
		resSvr = null;
		audioSvc = null;
        netSvc = null;
	}

	protected void SetText(Text text,string context="")
	{
		text.text = context;
	}

	protected void SetText(Transform transform,int num=0)
	{
		SetText(transform.GetComponent<Text>(), num);
	}

	protected void SetText(Transform transform,string context="")
	{
		SetText(transform.GetComponent<Text>(), context);
	}

	protected void SetText(Text text,int num=0)
	{
		SetText(text,num.ToString());
	}

	protected void SetActive(GameObject gameObject,bool isActive = true)
	{
		gameObject.SetActive(isActive);
	}

	protected void SetActive(Transform transform,bool isActive= true)
	{
		transform.gameObject.SetActive(isActive);
	}

	protected void SetActive(RectTransform rectTransform,bool isActive=true)
	{
		rectTransform.gameObject.SetActive(isActive);
	}

	protected void SetActive(Image image,bool isActive=true)
	{
		image.gameObject.SetActive(isActive);
	}

	protected void SetActive(Text text,bool isActive=true)
	{
		text.gameObject.SetActive(isActive);
	}

    protected T GetOrAddComponect<T>(GameObject go) where T:Component
    {
        T t = go.GetComponent<T>();
        if(t==null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    protected void OnClickDown(GameObject go,Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClickDown = cb;
    }

    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onDrag = cb;
    }

    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClickUp = cb;
    }


    public void SetSprite(Image img,string path)
    {
        Sprite sp = resSvr.LoadSprite(path, true);
        img.sprite = sp;
    }
}
