using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvr : MonoBehaviour {

	public static ResSvr instance;
	public void Init()
	{
		instance= this;
		InitRDNameCfg();

        Debug.Log("ResSvr Start");
	}
	private Action prgCB = null;

	public void AsyncLoadScene(string sceneName,Action action)
	{
        GameRoot.Instance.loadingWnd.SetWndState();
        AsyncOperation sceneOperator = SceneManager.LoadSceneAsync(sceneName);
		prgCB = () =>
		{
			float val = sceneOperator.progress;
			GameRoot.Instance.loadingWnd.SetProgress(val);
			if (val >= 1)
			{
				if(action!= null)
					action();
				prgCB = null;
                sceneOperator = null;
				GameRoot.Instance.loadingWnd.SetWndState(false);
			}
		};
		
	}

	public void Update()
	{
		if(prgCB != null)
		{
			prgCB();
		}
	}

	private Dictionary<string,AudioClip> adDic = new Dictionary<string,AudioClip>();

	public AudioClip LoadAudio(string path,bool cache = false)
	{
		Debug.LogError(path);
		AudioClip audioClip = null;
		if(!adDic.TryGetValue(path, out audioClip))
		{
			audioClip = Resources.Load<AudioClip>(path);
			if(cache)
			{
				adDic[path] = audioClip;	
			}
		}
		return audioClip;
	}

	private List<string> surnameList= new List<string>();
	private	List<string> manList = new List<string>();
	private List<string> woList = new List<string>();
	private void InitRDNameCfg()
	{
		TextAsset textAsset = Resources.Load<TextAsset>(PathDefine.RDNameCfg);
		if(textAsset != null )
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(textAsset.text);

			XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("root").ChildNodes;
			for(int i=0;i<xmlNodeList.Count;i++)
			{
				XmlElement xmlElement = xmlNodeList[i] as XmlElement;
				if (xmlElement.GetAttributeNode("ID") == null)
					continue;
				foreach(XmlElement e in xmlElement.ChildNodes)
				{
					switch(e.Name)
					{
						case "surname":
							surnameList.Add(e.InnerText);
							break;
						case "man":
							manList.Add(e.InnerText);
							break;
						case "woman":
							woList.Add(e.InnerText);
							break;
					}
				}
			}
		}
	}

	public string GetRDNameData(bool man = false)
	{
		System.Random rd = new System.Random();
		string rdName = surnameList[PETools.RDInt(0,surnameList.Count-1)];
		if(man)
		{
			rdName += manList[PETools.RDInt(0, manList.Count - 1)];
		}
		else
		{
			rdName += woList[PETools.RDInt(0, woList.Count - 1)];
		}
		return rdName;
	}
}
