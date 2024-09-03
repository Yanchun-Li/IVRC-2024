using UnityEngine;
using System;
using System.Linq;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class ChestCopy : MonoBehaviour
{
    public GameObject originalChest;
    public GameObject copyChest;
    public Vector3 difforigin=new Vector3(-400f,0f,0f);//手動で計算
    private Vector3 newChestPosition;
    private bool isProcessing=false;
    private ObjectDuplicator objectduplicator;
    private GameObject Player2Room;
    private GameObject Avatar1;

    // Start is called before the first frame update
    void Start()
    {
        Player2Room = GameObject.Find("Player2 Room");
        objectduplicator = Player2Room.GetComponent<ObjectDuplicator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectduplicator == null){
            Player2Room = GameObject.Find("Player2 Room");
            objectduplicator = Player2Room.GetComponent<ObjectDuplicator>();
        }
        // 指定されたボタンが押され、かつ現在処理中でない場合に実行
        if (OVRInput.GetDown(OVRInput.Button.One) && !isProcessing)
        {
            Debug.Log("push A button and copy chest");
            DuplicateChest();
        }
    }

    public void DuplicateChest()
    {
        if (isProcessing) return; // 既に処理中なら新たに開始しない
        isProcessing = true;
        while (objectduplicator.duplicatedObject == null){}//宝箱の移動をさけるため、部屋ができてから宝箱の生成に移る
        difforigin = objectduplicator.duplicatedObject.transform.position - Player2Room.transform.position;
        newChestPosition = originalChest.transform.position + difforigin;
        copyChest = Instantiate(originalChest, newChestPosition, originalChest.transform.rotation);
        Avatar1 = GameObject.Find("Avatar1(Clone)");
        Avatar1.GetComponent<ChestRayInteraction>().enabled = false;//アバター1はコピー世界にいるときは宝に関与できない
        StartCoroutine(UpdateAndDestroy());
    }

    private IEnumerator UpdateAndDestroy()
    {
        float duration = objectduplicator.duration;
        yield return new WaitForSeconds(duration);
        Destroy(copyChest);
        Avatar1.GetComponent<ChestRayInteraction>().enabled = true;
        isProcessing = false; // 処理完了
    }

}
