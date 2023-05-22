using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChatSys
{
    private static ChatSys instance = null;
    public static ChatSys Instance { get { if (instance == null) instance = new ChatSys(); return instance; } }

    public void Init()
    {
        PECommon.Log("Chat Sys Init");
    }

    internal void SndChat(MsgPack pack)
    {
        SndChat data = pack.GameMsg.sndChat;
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.SeverSession);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.pshChat,
            pshChat = new PshChat
            {
                name = pd.name,
                chat = data.chat
            }
        };

        List<SeverSession> lst = CacheSvc.Instance.GetOnlineServerSessions();
        //优化  类对象转二进制 再推送给所有玩家
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for(int i=0;i<lst.Count;i++)
        {
            lst[i].SendMsg(bytes);
        }
    }
}
