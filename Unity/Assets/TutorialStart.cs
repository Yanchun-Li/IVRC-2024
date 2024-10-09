using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TutorialStart : ITutorialTask
{
    public string GetTitle()
    {
        return "チュートリアル";
    }
 
    public string GetText()
    {
        return "これからチュートリアルを始めます。";
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