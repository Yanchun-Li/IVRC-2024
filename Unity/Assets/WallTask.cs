using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class WallTask : MonoBehaviour, ITutorialTask
{
    public GameObject wallprefab;
    public GameObject spawnwall;

    public string GetTitle()
    {
        return "基本操作 壁を消す";
    }
 
    public string GetText()
    {
        return "右のトリガーボタンを押すと壁を消すことができます";
    }
 
    public void OnTaskSetting()
    {
        wallprefab = Resources.Load<GameObject>("TutorialMovableWall");
        spawnwall = Instantiate(wallprefab, new Vector3(2.0f,0.0f,0.0f), Quaternion.identity);
    }
 
    public bool CheckTask()
    {
        if (spawnwall != null && spawnwall.activeSelf == false){
            return true;
        }   
 
        return false;
    }
 
    public float GetTransitionTime()
    {
        return 2f;
    }
}