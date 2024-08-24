using UnityEngine;
using Oculus.Interaction;
using System.Collections.Generic;

public class ChestRayInteraction : MonoBehaviour
{
    public int scoreIncrement = 1;
    private int score = 0;

    private List<InteractableUnityEventWrapper> chestInteractables = new List<InteractableUnityEventWrapper>();

    void Start()
    {
        // ゲーム開始時に既存のチェストを探して登録
        FindAndRegisterChests();
    }

    void FindAndRegisterChests()
    {
        // "chest"タグが付いているすべてのオブジェクトを見つける
        GameObject[] chestObjects = GameObject.FindGameObjectsWithTag("Chest");
        // while (chestObjects.Length ==0){
        //     chestObjects = GameObject.FindGameObjectsWithTag("Chest");
        // }
        // Debug.Log($"chest is {chestObjects}");

        foreach (GameObject chestObject in chestObjects)
        {
            RegisterChest(chestObject);
        }
    }

    void RegisterChest(GameObject chestObject)
    {
        InteractableUnityEventWrapper wrapper = chestObject.GetComponent<InteractableUnityEventWrapper>();
        if (wrapper != null)
        {
            chestInteractables.Add(wrapper);
            wrapper.WhenSelect.AddListener(OnChestSelected);
            Debug.Log($"Registered chest: {chestObject.name}");
        }
        else
        {
            Debug.LogWarning($"InteractableUnityEventWrapper not found on chest: {chestObject.name}");
        }
    }

    public void OnChestSelected()
    {
        // スコアを加算
        score += scoreIncrement;
        Debug.Log("Score: " + score);
    }

    // 新しいチェストが生成されたときに呼び出すメソッド
    public void OnNewChestSpawned(GameObject newChest)
    {
        RegisterChest(newChest);
        Debug.Log("chest is spawned");
    }

    private void OnDestroy()
    {
        // クリーンアップ
        foreach (var wrapper in chestInteractables)
        {
            if (wrapper != null)
            {
                wrapper.WhenSelect.RemoveListener(OnChestSelected);
            }
        }
    }
}