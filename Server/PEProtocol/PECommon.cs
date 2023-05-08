using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PENet;

namespace PEProtocol
{
    public enum LogType
    {
        Log = 0,
        Warn = 1,
        Error = 2,
        Info = 3,
    }

    public class PECommon
    {
        public static void Log(string msg = "", LogType tp = LogType.Log)
        {
            LogLevel logLevel = (LogLevel)tp;
            PETool.LogMsg(msg, logLevel);
        }

        public static int GetFightByProps(PlayerData playerData)
        {
            return playerData.lv * 100 + playerData.ad + playerData.addef + playerData.apdef;
        }

        public static int GetPowerLimit(int lv)
        {
            return ((lv - 1) / 10) * 150 + 150;
        }

        public static int GetExpUpValByLv(int lv)
        {
            return 100 * lv * lv;
        }
    }
}


