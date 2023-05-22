using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd : WindowRoot {

    public Text txtChat;
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;
    public InputField iptChat;

    private int chatType;
    private List<string> chatList = new List<string>();

    public override void InitWnd()
    {
        base.InitWnd();
        chatType = 0;
        RefreshUI();
    }

    public void AddChatMsg(string name,string chat)
    {
        chatList.Add(Const.Color(name + ":", TxtColor.Blue) + chat);
        if(chatList.Count>12)
        {
            chatList.RemoveAt(0);
        }
        if(GetWndState())
        {
            RefreshUI();
        }
    }

    void RefreshUI()
    {
        if(chatType==0)
        {
            string chatMsg = "";
            for(int i=0;i<chatList.Count;i++)
            {
                chatMsg += chatList[i] + "\n";
            }
            SetText(txtChat, chatMsg);

            SetSprite(imgWorld, "ResImages/btntype1");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if(chatType==1)
        {
            SetText(txtChat, "暂无公会");

            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype1");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if(chatType==2)
        {
            SetText(txtChat, "暂无好友");

            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype1");
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        SetWndState(false);
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }

    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }

    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Const.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }

    public void ClickSendBtn()
    {
        if(iptChat.text!=null && iptChat.text!=""&&iptChat.text!=" ")
        {
            if(iptChat.text.Length>12)
            {
                GameRoot.AddTips("输入信息不能超过12个字");
            }
            else
            {
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
            }
        }
        else
        {
            GameRoot.AddTips("请输入内容");
        }
    }
}
