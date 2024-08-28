using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

 public class PlayerMake : MonoBehaviourPunCallbacks
 {
    public bool player2exist = false;
     private void Start() {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        PhotonNetwork.ConnectUsingSettings();
     }

     public override void OnConnectedToMaster() {
         PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
         //PhotonNetwork.CreateRoom(null, new RoomOptions(), TypedLobby.Default);
     }

     public override void OnJoinedRoom() {
         var position = new Vector3(0,1.1f,0);
         var playerlist = new List<Player>(PhotonNetwork.PlayerList);
         GameObject playerInstance = null;
         if (playerlist.Count==1){
            playerInstance=PhotonNetwork.Instantiate("Avatar1", position, Quaternion.identity);
            PhotonNetwork.NickName = "Player1";
         }
         else if (playerlist.Count==2 & !player2exist){
            position = new Vector3(200,1.1f,0);
            playerInstance=PhotonNetwork.Instantiate("Avatar2", position, Quaternion.identity);
            PhotonNetwork.NickName = "Player2";
            player2exist = true;
         }
         else if (playerlist.Count==2 & player2exist){
            position = new Vector3(0,1.1f,0);
            playerInstance=PhotonNetwork.Instantiate("Avatar1", position, Quaternion.identity);
         }else{
            position = new Vector3(-1000f,0f,0f);
            playerInstance=PhotonNetwork.Instantiate("Avatar3", position, Quaternion.identity);
         }

        if (playerInstance != null)
        {
            ChestRayInteraction chestRayInteraction = playerInstance.GetComponent<ChestRayInteraction>();
            if (chestRayInteraction != null)
            {
                chestRayInteraction.InitializeScore();
            }
            else
            {
                Debug.LogError("ChestRayInteraction component not found on player instance");
            }
        }
        else
        {
            Debug.LogError("Failed to instantiate player");
        }
     }
 }