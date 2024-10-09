using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TutorialEnd : ITutorialTask
{
    public string GetTitle()
    {
        return "チュートリアル終了";
    }
 
    public string GetText()
    {
        return "これでチュートリアルは終了です。ゲーム画面に遷移します。";
    }
 
    public void OnTaskSetting()
    {
    }
 
    public bool CheckTask()
    {
        return true;
    }
 
    public float GetTransitionTime()
    {
        return 2f;
    }
}