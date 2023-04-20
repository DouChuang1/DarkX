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

        //服务
        NetSvc.Instance.Init();
        //业务
        LoginSys.Instance.Init();
    }

    public void Update()
    {
        NetSvc.Instance.Update();
    }
}
