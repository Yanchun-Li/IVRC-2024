using UnityEngine;
using Oculus.Interaction;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class ChestRayInteraction : MonoBehaviourPunCallbacks
{
    public int scoreIncrement = 1;
    public int myscore;

    public Timer timer;

    public OVRPlayerController oVRPlayerController;

    public GameObject ControllInteractor;


    private List<InteractableUnityEventWrapper> chestInteractables = new List<InteractableUnityEventWrapper>();

    void Start()
    {
        // ゲーム開始時に既存のチェストを探して登録
        FindAndRegisterChests();
        //UpdateScore(0);
        //myscore = 0;
    }

    public void InitializeScore()
    {
        myscore = 0;
        UpdateScore(myscore);
        Debug.Log($"Score initialized to {myscore}");
    }

    void Update()
    {
        int otherscore = -1;
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);
        if (playerlist.Count > 0)
        {
            foreach (Player player in playerlist)
            {
                if (!player.IsLocal)
                {
                    otherscore = GetPlayerScore(player);
                    Debug.Log($"other score is {otherscore}");
                }

                else if (player.IsLocal)
                {
                    //myscore = GetPlayerScore(player);
                    Debug.Log($"my score is {myscore}");
                }
            }
            //Debug.Log($"my score is {myscore}, and other score is {otherscore}");
        }
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
            wrapper.WhenSelect.AddListener(() => OnChestSelected(chestObject));
            Debug.Log($"Registered chest: {chestObject.name}");
        }
        else
        {
            Debug.LogWarning($"InteractableUnityEventWrapper not found on chest: {chestObject.name}");
        }
    }

    public void OnChestSelected(GameObject chestObject)
    {
        //部屋の位置によって点数変更（各部屋の中心から±8の領域に宝箱生成、余裕をもって±9とする）
        //部屋の中心：21番（260,-60）、22番（140,60）、23番（260,60）、24番（140,-60）
        Vector3 position = chestObject.transform.position;
        
        if (260 - 9 < position.x & position.x < 260 + 9 & -60 - 9 < position.z & position.z < -60 + 9)
        {
            scoreIncrement = 2;
        }
        else if (140 - 9 < position.x & position.x < 140 + 9 & 60 - 9 < position.z & position.z < 60 + 9)
        {
            scoreIncrement = 2;
        }
        else if (260 - 9 < position.x & position.x < 260 + 9 & 60 - 9 < position.z & position.z < 60 + 9)
        {
            scoreIncrement = 2;
        }else if (140-9<position.x  & position.x<140+9 & -60-9<position.z & position.z<-60+9){
            scoreIncrement = 2;
        }else if (-100 > position.x){
            scoreIncrement = 0;
        }
        else{
            scoreIncrement = 1;
        }
        // スコアを加算
        myscore += scoreIncrement;
        UpdateScore(myscore);
        Debug.Log("Score updated: " + myscore);
        // int updatedScore = GetScore();
        // Debug.Log($"After update, GetScore returns: {updatedScore}");
    }

    private System.Collections.IEnumerator CheckScoreAfterUpdate(int expectedScore)
    {
        yield return new WaitForSeconds(0.1f);
    }

    private void UpdateScore(int newScore)
    {
        //myscore = newScore;
        // Photonのカスタムプロパティを更新
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { "Score", newScore } };
        try
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            Debug.Log($"Attempting to set score in custom properties: {newScore}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error setting custom properties: {e.Message}");
        }

        StartCoroutine(CheckScoreAfterUpdate(newScore));

        // if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Score", out object setScore))
        // {
        //     Debug.Log($"Score successfully set in custom properties: {setScore}");
        // }
        // else
        // {
        //     Debug.LogError("Failed to set score in custom properties");
        // }
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
                wrapper.WhenSelect.RemoveListener(() => OnChestSelected(wrapper.gameObject));
            }
        }

        if (timer.isPlaying == false)
        {
            ControllInteractor.SetActive(false);
        }
    }

    public int GetScore()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Score", out object score))
        {
            Debug.Log($"we can get score: {score}");
            return (int)score;
        }
        return 0;
    }

    public int GetPlayerScore(Player player)
    {
        int ans = 0;
        if (player.CustomProperties.TryGetValue("Score", out object score))
        {
            ans = (int)score;
        }
        return ans;
    }

}