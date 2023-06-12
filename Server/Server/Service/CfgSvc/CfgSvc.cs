using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PENet;
using PEProtocol;

public class BaseData<T>
{
    public int ID;
}

public class MapCfg : BaseData<MapCfg>
{
    public int power;
}


public class GuideCfg :BaseData<GuideCfg>
{
    public int coin;
    public int exp;
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int startlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}
public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}


public class CfgSvc
{
    private static CfgSvc instance = null;
    public static CfgSvc Instance { get { if (instance == null) instance = new CfgSvc(); return instance; } }

    private Dictionary<int, GuideCfg> guideDic = new Dictionary<int, GuideCfg>();

    public void Init()
    {
        PECommon.Log("CfgSrc Init Done");
        InitGuideCfg();
        InitStrongCfg();
        InitTaskRewardCfg();
        InitMapCfg();
    }
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg()
    {

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(@"E:\TA\DarkX\Client\Assets\Resources\ResCfgs\map.xml");

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
                switch (e.Name)
                {
                    case "power":
                        mapCfg.power = int.Parse(e.InnerText);
                        break;
                }
            }
            mapCfgDataDic.Add(ID, mapCfg);
        }
    }

    public MapCfg GetMapCfgData(int mapId)
    {
        MapCfg mapCfg;
        if (mapCfgDataDic.TryGetValue(mapId, out mapCfg))
            return mapCfg;
        return null;
    }


    private void InitGuideCfg()
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(@"E:\TA\DarkX\Client\Assets\Resources\ResCfgs\guide.xml");

        XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            XmlElement xmlElement = xmlNodeList[i] as XmlElement;
            if (xmlElement.GetAttributeNode("ID") == null)
                continue;
            int ID = Convert.ToInt32(xmlElement.GetAttributeNode("ID").InnerText);
            GuideCfg agCfg = new GuideCfg
            {
                ID = ID
            };
            foreach (XmlElement e in xmlElement.ChildNodes)
            {
                switch (e.Name)
                {
                    case "coin":
                        agCfg.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        agCfg.exp = int.Parse(e.InnerText);
                        break;
                }
            }
            guideDic.Add(ID, agCfg);
        }
    }

    public GuideCfg GetGuideCfg(int id)
    {
        GuideCfg guideCfg = null;
        if(guideDic.TryGetValue(id,out guideCfg))
        {
            return guideCfg;
        }
        return null;
    }

    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"E:\TA\DarkX\Client\Assets\Resources\ResCfgs\strong.xml");

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

    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfg()
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(@"E:\TA\DarkX\Client\Assets\Resources\ResCfgs\taskreward.xml");

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
                    case "coin":
                        trc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        trc.exp = int.Parse(e.InnerText);
                        break;
                    case "count":
                        trc.count = int.Parse(e.InnerText);
                        break;
                }
            }
            taskRewardDic.Add(ID, trc);
        }
    }

    public TaskRewardCfg GetTaskRewardCfg(int id)
    {
        TaskRewardCfg taskRewardCfg = null;
        if (taskRewardDic.TryGetValue(id, out taskRewardCfg))
        {
            return taskRewardCfg;
        }
        return null;
    }
}
