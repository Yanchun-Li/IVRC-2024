using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerDataController : MonoBehaviourPunCallbacks
{
    [SerializeField] private ObjectPositionData player1positions;
    [SerializeField] private ObjectRotationData player1rotations;
    [SerializeField] private ObjectPositionData player2positions;
    [SerializeField] private ObjectRotationData player2rotations;
    public float recordInterval = 0.1f;
    private float timer = 0.0f;
    private float currenttime=0.0f;

    void Awake()
    {
        player1positions.isUpdating = true;
        player2positions.isUpdating = true;
        player1rotations.isUpdating = true;
        player2rotations.isUpdating = true;
    }

    void Update()
    {
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);

        timer += Time.deltaTime;
        if (timer > recordInterval){
            foreach (Player player in playerlist){
                if (player.NickName=="Player1"){
                    StartCoroutine(GetTransform(player, player1positions, player1rotations));
                }else if(player.NickName=="Player2"){
                    StartCoroutine(GetTransform(player, player2positions, player2rotations));
                }
            }
            timer = 0f;
        }
    }

    private IEnumerator GetTransform(Player player, ObjectPositionData otherpositions, ObjectRotationData otherrotations){
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
        Debug.Log($"add {otherposition} to {otherpositions}, ID:{player.ActorNumber}");
        yield return new WaitForSeconds(0.1f);
    }
}