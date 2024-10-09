using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TreasureTask : MonoBehaviour, ITutorialTask
{
    public GameObject chestprefab;
    public GameObject spawnchest;

    public string GetTitle()
    {
        return "基本操作 宝をゲット";
    }
 
    public string GetText()
    {
        return "右のトリガーボタンを押すと宝を獲得できます";
    }
 
    public void OnTaskSetting()
    {
        chestprefab = Resources.Load<GameObject>("TutorialChestParent");
        if (chestprefab != null){
            spawnchest = Instantiate(chestprefab, new Vector3(2.0f,0.0f,0.0f), Quaternion.identity);
        }
        else if (chestprefab == null)
        {
            Debug.LogError("Chest prefab is not assigned in the inspector");
        }
    }
 
    public bool CheckTask()
    {
        if (spawnchest != null && spawnchest.activeSelf == false){
            return true;
        }   
 
        return false;
    }
 
    public float GetTransitionTime()
    {
        return 2f;
    }
}