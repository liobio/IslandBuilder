using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverManager : Singleton<ObserverManager>
{

    private Dictionary<string, List<Action>> dicAll; //存储所有事件和响应
    //private List<string> curAct = new List<string>();//存储当前帧注册的事件key值
    //private ArrayList objArgs = new ArrayList();     //存储key对应的参数

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        dicAll = new Dictionary<string, List<Action>>();
    }
    public void AddListener(string key, Action act)
    {
        if (dicAll.ContainsKey(key))
        {
            dicAll[key].Add(act);
        }
        else
        {
            Debug.Log("未注册观察事件");

        }
    }
    public void Register(string key, object args = null)
    {
        if (dicAll.ContainsKey(key))
        {
            Debug.Log(key + "观察事件已被注册");
        }
        else
        {
            List<Action> actions = new List<Action>();
            dicAll.Add(key, actions);
        }
    }
    public void ReMoveListener(string key, Action act)
    {
        if (dicAll.ContainsKey(key))
        {
            dicAll[key].Remove(act);
        }
        else
        {
            Debug.Log("未注册观察事件");

        }
    }
    public void RespondListener(string key)
    {
        if (dicAll.ContainsKey(key))
        {
            foreach (Action item in dicAll[key])
            {
                item.Invoke();
            }
        }
        else
        {
            Debug.Log(key + "事件不存在");
        }
    }
}

