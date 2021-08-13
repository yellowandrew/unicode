using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHandler : MsgHandler
{
    public LoginHandler(UnityApp app,int tag) : base(app,tag) {
        
    }
    public override void OnLocalMessage(int msgCode, object message)
    {
        switch (msgCode)
        {
            default:
                break;
        }
    }

    public override void OnNetMessage(SocketMSG msg)
    {
        Debug.Log("登录模块接收到服务器消息");
        msg.print();
        app.EmitMessage(msg.Command,"登录成功进入战斗！");
    }
}
