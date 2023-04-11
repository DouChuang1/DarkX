using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour {

	public ResSvr resSvr;
	public AudioSvc audioSvc;
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
	}

	public void ClearWnd()
	{
		resSvr = null;
		audioSvc = null;	
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

}
