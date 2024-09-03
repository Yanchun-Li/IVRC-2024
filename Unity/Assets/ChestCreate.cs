using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChestCreate : MonoBehaviourPunCallbacks
{
    [SerializeField] int number = 10;//宝箱の数
    [SerializeField] GameObject chestParent;
    [SerializeField] GameObject CopyWorld;//コピーすべき先
    private bool createTreasure = false;//宝の生成は1ゲームにつき1回
    public List<GameObject> chests = new List<GameObject>();
    private ChestRayInteraction chestRayInteraction;
    // Start is called before the first frame update
    void Start()
    {
        chestRayInteraction = FindObjectOfType<ChestRayInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);
        if (playerlist.Count == 1 & createTreasure == false){
            MakeTresure();
        }if (playerlist.Count == 0){
            DestroyTreasure();
        }
    }

    public void MakeTresure(){
        int[] randomNumbers = Enumerable.Range(0, 20).OrderBy(_ => Guid.NewGuid()).ToArray();
        int[] cornerNumbers = new int[] {21,22,23,24};
        System.Collections.Generic.List<int>
        mergedList = new System.Collections.Generic.List<int>(randomNumbers.Length + cornerNumbers.Length);
        //配列をコレクションに追加する
        
        mergedList.AddRange(cornerNumbers);
        mergedList.AddRange(randomNumbers);

        //配列に変換する
        int[] mergedArray = mergedList.ToArray();

        for (int i=0;i<number;i++){
            System.Random rnd = new System.Random();
            float x = rnd.Next(-8,8);
            float z = rnd.Next(-8,8);
            //float x = 0;
            //float z = 0;
            Vector3 localposition = new Vector3(x,0.1f,z);
            Transform origin = this.transform.GetChild(mergedArray[i]);
            Transform Copyorigin = CopyWorld.transform.GetChild(mergedArray[i]);
            Vector3 originalPosition = origin.position;
            Vector3 CopyoriginalPosition = Copyorigin.position;
            GameObject chest1 = PhotonNetwork.Instantiate("Chest Parent 001",localposition+originalPosition,Quaternion.identity);
            GameObject chest2 = PhotonNetwork.Instantiate("Chest Parent 001",localposition+CopyoriginalPosition,Quaternion.identity);
            // GameObject chest1 = Instantiate(chestParent,localposition+originalPosition,Quaternion.identity);
            // GameObject chest2 =Instantiate(chestParent,localposition+CopyoriginalPosition,Quaternion.identity);
            chests.Add(chest1);
            chests.Add(chest2);
            if (chestRayInteraction != null)
            {
                chestRayInteraction.OnNewChestSpawned(chest1);
                chestRayInteraction.OnNewChestSpawned(chest2);
            }
        }
        createTreasure = true;
    }

    
    public void DestroyTreasure()
        {
            foreach (GameObject chest in chests)
            {
                Destroy(chest);
            }
        }
}
