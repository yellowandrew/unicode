using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AppLauncher : MonoBehaviour
{
   
    [Header("应用/游戏主类名")]
    public string AppClass= "UnityApp";
    UnityApp app;

    void Awake()
    {
        app = Assembly.GetExecutingAssembly().CreateInstance(AppClass) as UnityApp;
    }
    void Start()
    {
        app.init(this);
        app.start();
    }

   
    void Update()
    {
        app.update(Time.deltaTime);
    }

    void FixedUpdate()
    {
        app.fixedUpdate(Time.deltaTime);
    }
    void OnApplicationQuit()
    {
        app.exit();
    }

    void OnApplicationFocus(bool focus)
    {
        app.focus(focus);
    }

    void OnApplicationPause(bool pause)
    {
        app.pause(pause);
    }
}
