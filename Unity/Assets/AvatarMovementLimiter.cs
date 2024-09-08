using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class AvatarMovementLimiter : MonoBehaviour
{
    private ObjectDuplicator objectDuplicator; 
    public ObjectPositionData objectPositionData; 
    public GameObject duplicatedAvatar;
    private Vector3 player1position;
    private float judgement_time;
    GameObject OVRPlayerController;
    private float radius = 8f;  // 移動可能な半径を2mに設定

    void Start()
    {
        objectDuplicator = GameObject.Find("Player2 Room Copy").GetComponent<ObjectDuplicator>();
        duplicatedAvatar = GameObject.Find("Avatar1(Clone)");
        OVRPlayerController = GameObject.Find("OVRPlayerController");
        judgement_time = 0;
    }

    void Update()
    {
        judgement_time += Time.deltaTime;
        if (duplicatedAvatar == null){
            duplicatedAvatar = GameObject.Find("Avatar1(Clone)");
        }
        if (objectDuplicator == null || objectPositionData == null)
        {
            Debug.Log("AML-objectDuplicator" + objectDuplicator);
            Debug.Log("AML-objectPositionData" + objectPositionData);
            return; // objectDuplicatorまたはobjectPositionDataがnullなら何もしない
        }
        Debug.Log("AML-objectDuplicator.duplicatedAvatar:" + objectDuplicator.duplicatedAvatar);
        Debug.Log("AML-objectPositionData.LengthPositions:" + objectPositionData.LengthPositions());
        
        if (objectDuplicator.duplicatedAvatar != null && objectPositionData.LengthPositions() > 0)
        {
            Vector3 avatarPosition = objectDuplicator.duplicatedAvatar.transform.position;  // duplicatedAvatarの現在位置を取得
            player1position = objectPositionData.GetPosition(objectPositionData.LengthPositions() - 1);  // 最新の位置を取得

            Debug.Log("AML-avatarPosition:" + avatarPosition);
            Debug.Log("AML-player1position:" + player1position);

            float distanceFromCenter = Vector3.Distance(player1position, avatarPosition);  // 円心からduplicatedAvatarまでの距離を計算
            Debug.Log("AML-distanceFromCenter:" + distanceFromCenter);
            Debug.Log("AML-judgement time " +judgement_time);

            if (judgement_time >= 500f){
                // Debug.Log("AML-judgement time " +judgement_time);
                if (distanceFromCenter > radius)  // 半径を超えたら、可動範囲に戻す
                {
                    Debug.Log($"AML-distanceFromCenter is:{distanceFromCenter}, radius is:{radius}");
                    Vector3 direction = (player1position - avatarPosition).normalized;  // 円心からの方向ベクトル
                    Debug.Log("AML-directon" + direction);
                    player1position = objectDuplicator.duplicatedAvatar.transform.position + direction * 0.125f * radius;  // エッジに戻す
                    Debug.Log("AML-corrected player1position:" + player1position);
                    
                    duplicatedAvatar.transform.position = player1position;
                    OVRPlayerController.GetComponent<CharacterController>().enabled = false;
                    OVRPlayerController.GetComponent<OVRPlayerController>().enabled = false;
                    OVRPlayerController.transform.position = duplicatedAvatar.transform.position;
                    OVRPlayerController.transform.rotation = duplicatedAvatar.transform.rotation;
                    OVRPlayerController.GetComponent<CharacterController>().enabled = true;
                    OVRPlayerController.GetComponent<OVRPlayerController>().enabled = true;
                }
                judgement_time = 0;
            }

        }
    }
}

// using UnityEngine;
// using Photon.Pun;
// using UnityEngine.UI;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Photon.Pun.Demo.Cockpit;
// using Photon.Realtime;

// public class AvatarMovementLimiter : MonoBehaviour
// {
//     private ObjectDuplicator objectDuplicator; 
//     public ObjectPositionData objectPositionData; 
//     public GameObject duplicatedAvatar;
//     private Vector3 player1position;
//     GameObject OVRPlayerController;
//     private float radius = 8f;  // 移動可能な半径を設定
//     private float bufferDistance = 0.5f;  // 境界に近づいたときのバッファ
//     private float correctionSpeed = 5f;  // ポジション修正の速度
//     private bool isCorrecting = false;  // ポジションを修正中かどうか

//     void Start()
//     {
//         objectDuplicator = GameObject.Find("Player2 Room Copy").GetComponent<ObjectDuplicator>();
//         duplicatedAvatar = GameObject.Find("Avatar1(Clone)");
//         OVRPlayerController = GameObject.Find("OVRPlayerController");
//     }

//     void Update()
//     {
//         if (duplicatedAvatar == null){
//             duplicatedAvatar = GameObject.Find("Avatar1(Clone)");
//         }
//         if (objectDuplicator == null || objectPositionData == null)
//         {
//             Debug.Log("AML-objectDuplicator" + objectDuplicator);
//             Debug.Log("AML-objectPositionData" + objectPositionData);
//             return; // objectDuplicatorまたはobjectPositionDataがnullなら何もしない
//         }
        
//         if (objectDuplicator.duplicatedAvatar != null && objectPositionData.LengthPositions() > 0)
//         {
//             Vector3 avatarPosition = objectDuplicator.duplicatedAvatar.transform.position;  // duplicatedAvatarの現在位置を取得
//             player1position = objectPositionData.GetPosition(objectPositionData.LengthPositions() - 1);  // 最新の位置を取得

//             float distanceFromCenter = Vector3.Distance(player1position, avatarPosition);  // 円心からduplicatedAvatarまでの距離を計算

//             if (distanceFromCenter > radius + bufferDistance)  // 半径 + バッファを超えたら修正
//             {
//                 if (!isCorrecting)  // 修正中でない場合のみ修正開始
//                 {
//                     isCorrecting = true;
//                     StartCoroutine(CorrectPosition(avatarPosition, player1position));
//                 }
//             }
//         }
//     }

//     private IEnumerator CorrectPosition(Vector3 avatarPosition, Vector3 player1position)
//     {
//         // プレイヤー入力を無効化
//         OVRPlayerController.GetComponent<CharacterController>().enabled = false;
//         OVRPlayerController.GetComponent<OVRPlayerController>().enabled = false;

//         Vector3 direction = (player1position - avatarPosition).normalized;
//         Vector3 targetPosition = avatarPosition + direction * 0.5f * radius;

//         // 無限大やNaNをチェック
//         if (float.IsNaN(targetPosition.x) || float.IsNaN(targetPosition.y) || float.IsNaN(targetPosition.z))
//         {
//             Debug.LogError("Invalid targetPosition: " + targetPosition);
//             yield break;
//         }

//         while (Vector3.Distance(duplicatedAvatar.transform.position, targetPosition) > 0.1f)
//         {
//             duplicatedAvatar.transform.position = Vector3.Lerp(duplicatedAvatar.transform.position, targetPosition, Time.deltaTime * correctionSpeed);
//             OVRPlayerController.transform.position = duplicatedAvatar.transform.position;
//             OVRPlayerController.transform.rotation = duplicatedAvatar.transform.rotation;
//             yield return null;
//         }

//         // プレイヤー入力を再度有効化
//         OVRPlayerController.GetComponent<CharacterController>().enabled = true;
//         OVRPlayerController.GetComponent<OVRPlayerController>().enabled = true;

//         isCorrecting = false;
//     }
// }
