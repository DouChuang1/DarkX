using PEProtocol;
using System;
using System.Collections.Generic;
using System.Text;

public class ServerRoot
{
    private static ServerRoot instance = null;
    public static ServerRoot Instance { get { if (instance == null) instance = new ServerRoot(); return instance; } }

    public void Init()
    {
        //数据库初始化 
        DBMgr.Instance.Init();
        //服务
        NetSvc.Instance.Init();
        CfgSvc.Instance.Init();
        TimerSvc.Instance.Init();
        //业务
        LoginSys.Instance.Init();
        //引导
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
        FubenSys.Instance.Init();

        try
        {
            try
            {
                Test(10, 0);
            }
            catch
            {
                throw new Exception("被除数不能为0");
            }
            throw new Exception("测试");
        }
        catch(Exception e)
        {
            PECommon.Log("Test-----" + e);
        }
    }

    public int  Test(int a,int b)
    {
        return a / b;
    }

    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int seeesionId = 0;
    public int GetSessionID()
    {
        if(seeesionId==int.MaxValue)
        {
            seeesionId = 0;
        }
        seeesionId = seeesionId + 1;
        return seeesionId;
    }
}
