using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStudy : MonoBehaviour
{
    List<Action> PatternActions = new List<Action>();
    Action func;

    public void Init()
    {
        PatternActions.Add(Action1);
        PatternActions.Add(Action2);
    }

    public void RandomActions()
    {
        int idx = UnityEngine.Random.Range(0, PatternActions.Count);
        PatternActions[idx]();
    }

    void Action1()
    {
        Debug.Log("Action1");
    }

    void Action2()
    {
        Debug.Log("Action2");
    }

    public void FuncAction()
    {
        func = FuncTemp;
        func += FuncTemp2;
        FuncParameter(func);
    }

    public void FuncParameter(Action action)
    {
        action();
    }

    void FuncTemp()
    {
        Debug.Log("Action Function");
    }

    void FuncTemp2()
    {
        Debug.Log("Extra Function");
    }
}
