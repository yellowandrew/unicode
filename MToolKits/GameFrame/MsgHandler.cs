using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MsgHandler
{
    public int tag;
    protected UnityApp app;
    public MsgHandler(UnityApp app,int tag) {
        this.app = app;
        this.tag = tag;
    }

    public virtual void init() { }
    protected void RegisterMessage(int msgCode) {
        app.BindMessage(tag,msgCode);
    }
    protected void UnRegisterMessage(int msgCode)
    {
        app.RemoveMessage(msgCode,this);
    }
    public abstract void OnLocalMessage(int msgCode, object message);

    public abstract void OnNetMessage(SocketMSG msg);
}
