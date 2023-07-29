using PEProtocol;
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
        InitMonsterCfg(PathDefine.MonsterCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.AutoGuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        InitTaskRewardCfg(PathDefine.TaskRewardCfg);
        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
        InitSkillActionCfg(PathDefine.SkillActionCfg);
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
                    ID = ID,
                    monsterList = new List<MonsterData>()
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch(e.Name)
                    {
                        case "mapName":
                            mapCfg.mapName = e.InnerText;
                            break;
                        case "power":
                            mapCfg.power = int.Parse(e.InnerText);
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
                        case "monsterLst":
                            string[] Arr1 = e.InnerText.Split('#');
                            for(int waveIndex=0;waveIndex<Arr1.Length;waveIndex++)
                            {
                                if (waveIndex == 0)
                                    continue;
                                string[] Arr2 = Arr1[waveIndex].Split('|');
                                for(int j=0;j<Arr2.Length;j++)
                                {
                                    if (j == 0)
                                        continue;
                                    string[] Arr3 = Arr2[j].Split(',');
                                    MonsterData md = new MonsterData
                                    {
                                        ID = int.Parse(Arr3[0]),
                                        mWave = waveIndex,
                                        mIndex = j,
                                        mCfg = GetMonsterCfg(int.Parse(Arr3[0])),
                                        mBornPos = new Vector3(float.Parse(Arr3[1]), float.Parse(Arr3[2]), float.Parse(Arr3[3])),
                                        mBornRote = new Vector3(0, float.Parse(Arr3[4]), 0),
                                        mLevel = int.Parse(Arr3[5]),
                                    };
                                    mapCfg.monsterList.Add(md);
                                }

                            }
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

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    private Dictionary<int, AutoGuideCfg> guideCfgDict = new Dictionary<int, AutoGuideCfg>();

    private void InitGuideCfg(string path)
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
                AutoGuideCfg agCfg = new AutoGuideCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            agCfg.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            agCfg.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            agCfg.actID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            agCfg.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            agCfg.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                guideCfgDict.Add(ID, agCfg);
            }
        }
    }

    public AutoGuideCfg GetAutoGuideData(int id)
    {
        AutoGuideCfg agc = null;
        if(guideCfgDict.TryGetValue(id,out agc))
        {
            return agc;
        }
        return null;
    }

    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + " not exist", PEProtocol.LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                StrongCfg sd = new StrongCfg
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    int val = int.Parse(e.InnerText);
                    switch (e.Name)
                    {
                        case "pos":
                            sd.pos = val;
                            break;
                        case "starlv":
                            sd.startlv = val;
                            break;
                        case "addhp":
                            sd.addhp = val;
                            break;
                        case "addhurt":
                            sd.addhurt = val;
                            break;
                        case "adddef":
                            sd.adddef = val;
                            break;
                        case "minlv":
                            sd.minlv = val;
                            break;
                        case "coin":
                            sd.coin = val;
                            break;
                        case "crystal":
                            sd.crystal = val;
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongDic.TryGetValue(sd.pos, out dic))
                {
                    dic.Add(sd.startlv, sd);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(sd.startlv, sd);

                    strongDic.Add(sd.pos, dic);
                }
            }
        }
    }
    public StrongCfg GetStrongData(int pos, int starlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(starlv))
            {
                sd = dic[starlv];
            }
        }
        return sd;
    }

    public int GetPropAddValPreLv(int pos,int starLv,int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if(strongDic.TryGetValue(pos,out posDic))
        {
            for(int i=0;i<starLv;i++)
            {
                StrongCfg sd;
                if(posDic.TryGetValue(i,out sd))
                {
                    switch(type)
                    {
                        case 1:
                            val += sd.addhp;
                            break;
                        case 2:
                            val += sd.addhurt;
                            break;
                        case 3:
                            val += sd.adddef;
                            break;
                    }
                }
            }
        }
        return val;
    }
    private Dictionary<int, TaskRewardCfg> taskRewardDict = new Dictionary<int, TaskRewardCfg>();

    private void InitTaskRewardCfg(string path)
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
                TaskRewardCfg trc = new TaskRewardCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "count":
                            trc.count = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            trc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            trc.exp = int.Parse(e.InnerText);
                            break;
                        case "taskName":
                            trc.taskName = e.InnerText;
                            break;
                    }
                }
                taskRewardDict.Add(ID, trc);
            }
        }
    }

    public TaskRewardCfg GetTaskRewardData(int id)
    {
        TaskRewardCfg trc = null;
        if (taskRewardDict.TryGetValue(id, out trc))
        {
            return trc;
        }
        return null;
    }

    private Dictionary<int, SkillCfg> skillCfgDict = new Dictionary<int, SkillCfg>();

    private void InitSkillCfg(string path)
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
                SkillCfg skillCfg = new SkillCfg
                {
                    ID = ID,
                    skillMoveLst = new List<int>(),
                    skillActionLst = new List<int>(),
                    skillDamageLst=new List<int>(),
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "skillName":
                            skillCfg.skillName = e.InnerText;
                            break;
                        case "isCombo":
                            skillCfg.isCombo = e.InnerText.Equals("1");
                            break;
                        case "isCollide":
                            skillCfg.isCollide = e.InnerText.Equals("1");
                            break;
                        case "isBreak":
                            skillCfg.isBreak = e.InnerText.Equals("1");
                            break;
                        case "cdTime":
                            skillCfg.cdTime = int.Parse(e.InnerText);
                            break;
                        case "skillTime":
                            skillCfg.skillTime = int.Parse(e.InnerText);
                            break;
                        case "aniAction":
                            skillCfg.aniAction = int.Parse(e.InnerText);
                            break;
                        case "fx":
                            skillCfg.fx = e.InnerText;
                            break;
                        case "dmgType":
                            if(e.InnerText.Equals("1"))
                            {
                                skillCfg.dmgType = DamageType.AD;
                            }
                            else if(e.InnerText.Equals("2"))
                            {
                                skillCfg.dmgType = DamageType.AP;
                            }
                            else
                            {
                                skillCfg.dmgType = DamageType.None;
                            }
                            break;
                        case "skillMoveLst":
                            string[] skMoveArr = e.InnerText.Split('|');
                            for(int j=0;j<skMoveArr.Length;j++)
                            {
                                skillCfg.skillMoveLst.Add(int.Parse(skMoveArr[j]));
                            }
                            break;
                        case "skillActionLst":
                            string[] skActionArr = e.InnerText.Split('|');
                            for (int j = 0; j < skActionArr.Length; j++)
                            {
                                skillCfg.skillActionLst.Add(int.Parse(skActionArr[j]));
                            }
                            break;
                        case "skillDamageLst":
                            string[] skDamageArr = e.InnerText.Split('|');
                            for (int j = 0; j < skDamageArr.Length; j++)
                            {
                                skillCfg.skillDamageLst.Add(int.Parse(skDamageArr[j]));
                            }
                            break;
                    }
                }
                skillCfgDict.Add(ID, skillCfg);
            }
        }
    }

    public SkillCfg GetSkillCfg(int id)
    {
        SkillCfg sc = null;
        if (skillCfgDict.TryGetValue(id, out sc))
        {
            return sc;
        }
        return null;
    }

    private Dictionary<int, SkillMoveCfg> skillMoveCfgDict = new Dictionary<int, SkillMoveCfg>();

    private void InitSkillMoveCfg(string path)
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
                SkillMoveCfg smCfg = new SkillMoveCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "moveTime":
                            smCfg.moveTime = int.Parse(e.InnerText);
                            break;
                        case "moveDis":
                            smCfg.moveDis = float.Parse(e.InnerText);
                            break;
                        case "delayTime":
                            smCfg.delayTime = int.Parse(e.InnerText);
                            break;

                    }
                }
                skillMoveCfgDict.Add(ID, smCfg);
            }
        }
    }

    public SkillMoveCfg GetSkillMoveCfg(int id)
    {
        SkillMoveCfg smc = null;
        if (skillMoveCfgDict.TryGetValue(id, out smc))
        {
            return smc;
        }
        return null;
    }

    private Dictionary<int, SkillActionCfg> skillActionCfgDict = new Dictionary<int, SkillActionCfg>();

    private void InitSkillActionCfg(string path)
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
                SkillActionCfg saCfg = new SkillActionCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "delayTime":
                            saCfg.delayTime = int.Parse(e.InnerText);
                            break;
                        case "radius":
                            saCfg.radius = float.Parse(e.InnerText);
                            break;
                        case "angle":
                            saCfg.angle = int.Parse(e.InnerText);
                            break;

                    }
                }
                skillActionCfgDict.Add(ID, saCfg);
            }
        }
    }

    public SkillActionCfg GetSkillActionCfg(int id)
    {
        SkillActionCfg sa = null;
        if (skillActionCfgDict.TryGetValue(id, out sa))
        {
            return sa;
        }
        return null;
    }

    private Dictionary<int, MonsterCfg> monsterCfgDict = new Dictionary<int, MonsterCfg>();

    private void InitMonsterCfg(string path)
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
                MonsterCfg mCfg = new MonsterCfg
                {
                    ID = ID,
                    bps = new BattleProps(),
                };
                foreach (XmlElement e in xmlElement.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mName":
                            mCfg.mName = e.InnerText;
                            break;
                        case "resPath":
                            mCfg.resPath = e.InnerText;
                            break;
                        case "mType":
                           if(e.InnerText.Equals("1"))
                            {
                                mCfg.mType = MonsterType.Normal;
                            }
                           else if(e.InnerText.Equals("2"))
                            {
                                mCfg.mType = MonsterType.Boss;
                            }
                            break;
                        case "isStop":
                            mCfg.isStop = e.InnerText.Equals("1");
                            break;
                        case "skillID":
                            mCfg.skillID = int.Parse(e.InnerText);
                            break;
                        case "atkDis":
                            mCfg.atkDis = float.Parse(e.InnerText);
                            break;
                        case "hp":
                            mCfg.bps.hp = int.Parse(e.InnerText);
                            break;
                        case "ad":
                            mCfg.bps.ad = int.Parse(e.InnerText);
                            break;
                        case "ap":
                            mCfg.bps.ap = int.Parse(e.InnerText);
                            break;
                        case "addef":
                            mCfg.bps.addef = int.Parse(e.InnerText);
                            break;
                        case "apdef":
                            mCfg.bps.apdef = int.Parse(e.InnerText);
                            break;
                        case "dodge":
                            mCfg.bps.dodge = int.Parse(e.InnerText);
                            break;
                        case "pierce":
                            mCfg.bps.pierce = int.Parse(e.InnerText);
                            break;
                        case "critical":
                            mCfg.bps.critical = int.Parse(e.InnerText);
                            break;
                  
                    }
                }
                monsterCfgDict.Add(ID, mCfg);
            }
        }
    }

    public MonsterCfg GetMonsterCfg(int id)
    {
        MonsterCfg smc = null;
        if (monsterCfgDict.TryGetValue(id, out smc))
        {
            return smc;
        }
        return null;
    }

}
