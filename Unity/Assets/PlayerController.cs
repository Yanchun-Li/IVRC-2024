using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView photonView;
    private ChestRayInteraction chestRayInteraction;
    [SerializeField] private ObjectPositionData otherpositions;
    [SerializeField] private ObjectRotationData otherrotations;
    public float recordInterval = 0.1f;
    //private float timer = 0.0f;
    //private float currenttime=0.0f;

    void Awake()
    {
        otherpositions.isUpdating = true;
        otherrotations.isUpdating = true;
        photonView = GetComponent<PhotonView>();
        chestRayInteraction = GetComponent<ChestRayInteraction>();
    }

    void Update()
    {
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);
        if (photonView.IsMine)
        {
            // 移動後に位置情報を更新
            UpdatePlayerTransform();
        }

        // timer += Time.deltaTime;
        // if (timer > recordInterval){
        //     foreach (Player player in playerlist){
        //         if (!player.IsLocal){
        //             //カスタムプロパティに保存されている位置と座標を取得
        //             //相手のPositionDataとRotationDataに保存（0.1秒ごとに呼び出す）
        //             StartCoroutine(GetTransform(player));
        //         }
        //     }
        //     timer = 0f;
        // }
    }

    private IEnumerator GetTransform(Player player){
        Vector3 otherposition = Vector3.zero;
        if (player.CustomProperties.TryGetValue("PosX", out object posx) &&
            player.CustomProperties.TryGetValue("PosY", out object posy) &&
            player.CustomProperties.TryGetValue("PosZ", out object posz))
        {
            otherposition = new Vector3((float)posx, (float)posy, (float)posz);
        }else{
            Debug.Log("no available position data");
        }
        Quaternion otherrotation = Quaternion.identity;
         if (player.CustomProperties.TryGetValue("RotX", out object rotx) &&
            player.CustomProperties.TryGetValue("RotY", out object roty) &&
            player.CustomProperties.TryGetValue("RotZ", out object rotz) &&
            player.CustomProperties.TryGetValue("RotW", out object rotw)) 
        {
            otherrotation = new Quaternion((float)rotx, (float)roty, (float)rotz, (float)rotw);
        }else{
            Debug.Log("no available rotation data");
        }
        otherpositions.AddPosition(otherposition);
        otherrotations.AddRotation(otherrotation);
        Debug.Log($"add {otherposition} to {otherpositions}, ID:{photonView.OwnerActorNr}");
        yield return new WaitForSeconds(0.1f);
    }

    void UpdatePlayerTransform()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("PosX", position.x);
        hash.Add("PosY", position.y);
        hash.Add("PosZ", position.z);
        hash.Add("RotX", rotation.x);
        hash.Add("RotY", rotation.y);
        hash.Add("RotZ", rotation.z);
        hash.Add("RotW", rotation.w);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            //transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}