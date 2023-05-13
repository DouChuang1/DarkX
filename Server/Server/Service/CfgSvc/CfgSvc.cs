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

public class GuideCfg :BaseData<GuideCfg>
{
    public int coin;
    public int exp;
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
}
