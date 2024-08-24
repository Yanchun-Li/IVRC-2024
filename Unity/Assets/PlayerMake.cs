using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

 public class PlayerMake : MonoBehaviourPunCallbacks
 {
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
         if (playerlist.Count==1){
            PhotonNetwork.Instantiate("Avatar1", position, Quaternion.identity);
         }
         if (playerlist.Count==2){
            position = new Vector3(200,1.1f,0);
            PhotonNetwork.Instantiate("Avatar2", position, Quaternion.identity);
         }
        
     }
 }