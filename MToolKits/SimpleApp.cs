using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleApp : UnityApp
{

    public override void update(float delta)
    {
        base.update(delta);
        if (Input.GetKeyDown(KeyCode.C))
        {
            netManager.ConnectServer();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            netManager.SendData(1, 2, 3, "测试数据");
        }
    }

    protected override void SetHandlers()
    {
        addHandler(new LoginHandler(this,1));
        addHandler(new FightHandler(this, 2));
    }
}
