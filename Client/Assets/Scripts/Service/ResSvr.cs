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
		InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
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
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
	private void InitRDNameCfg(string path)
	{
		TextAsset textAsset = Resources.Load<TextAsset>(path);
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
        string rdName = surnameList[PETools.RDInt(0, surnameList.Count - 1)];
        if (man)
        {
            rdName += manList[PETools.RDInt(0, manList.Count - 1)];
        }
        else
        {
            rdName += woList[PETools.RDInt(0, woList.Count - 1)];
        }
        return rdName;
    }

    private void InitMapCfg(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        if (textAsset != null)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement xmlElement = xmlNodeList[i] as XmlElement;
                if (xmlElement.GetAttributeNode("ID") == null)
                    continue;
                int ID = Convert.ToInt32(xmlElement.GetAttributeNode("ID").InnerText);
                MapCfg mapCfg = new MapCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch(e.Name)
                    {
                        case "mapName":
                            mapCfg.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mapCfg.sceneName = e.InnerText;
                            break;
                        case "mainCamPos":
                            string[] valArr1 = e.InnerText.Split(',');
                            mapCfg.mainCamPos = new Vector3(float.Parse(valArr1[0]), float.Parse(valArr1[1]), float.Parse(valArr1[2]));
                            break;
                        case "mainCamRote":
                            string[] valArr2 = e.InnerText.Split(',');
                            mapCfg.mainCamRote = new Vector3(float.Parse(valArr2[0]), float.Parse(valArr2[1]), float.Parse(valArr2[2]));
                            break;
                        case "playerBornPos":
                            string[] valArr3 = e.InnerText.Split(',');
                            mapCfg.playerBornPos = new Vector3(float.Parse(valArr3[0]), float.Parse(valArr3[1]), float.Parse(valArr3[2]));
                            break;
                        case "playerBornRote":
                            string[] valArr4 = e.InnerText.Split(',');
                            mapCfg.playerBornRote = new Vector3(float.Parse(valArr4[0]), float.Parse(valArr4[1]), float.Parse(valArr4[2]));
                            break;
                    }
                }
                mapCfgDataDic.Add(ID, mapCfg);
            }
        }
    }

    public MapCfg GetMapCfgData(int mapId)
    {
        MapCfg mapCfg;
        if (mapCfgDataDic.TryGetValue(mapId, out mapCfg))
            return mapCfg;
        return null;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path,bool cache=false)
    {
        GameObject prefab = null;
        if(!goDic.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if(cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if(prefab!=null)
        {
            go = Instantiate(prefab);
        }

        return go;
    }
}
