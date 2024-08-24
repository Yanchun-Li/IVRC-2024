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
    // Start is called before the first frame update
    void Start()
    {

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
        int[] randomNumbers = Enumerable.Range(0, 24).OrderBy(_ => Guid.NewGuid()).ToArray();
        
        for (int i=0;i<number;i++){
            System.Random rnd = new System.Random();
            //float x = rnd.Next(-8,8);
            //float z = rnd.Next(-8,8);
            float x = 0;
            float z = 0;
            Vector3 localposition = new Vector3(x,0,z);
            Transform origin = this.transform.GetChild(randomNumbers[i]);
            Transform Copyorigin = CopyWorld.transform.GetChild(randomNumbers[i]);
            Vector3 originalPosition = origin.position;
            Vector3 CopyoriginalPosition = Copyorigin.position;
            GameObject chest1 = PhotonNetwork.Instantiate("Chest Parent 001",localposition+originalPosition,Quaternion.identity);
            GameObject chest2 = PhotonNetwork.Instantiate("Chest Parent 001",localposition+CopyoriginalPosition,Quaternion.identity);
            // GameObject chest1 = Instantiate(chestParent,localposition+originalPosition,Quaternion.identity);
            // GameObject chest2 =Instantiate(chestParent,localposition+CopyoriginalPosition,Quaternion.identity);
            chests.Add(chest1);
            chests.Add(chest2);
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
