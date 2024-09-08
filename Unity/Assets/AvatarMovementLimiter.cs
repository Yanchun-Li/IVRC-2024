using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class AvatarMovementLimiter : MonoBehaviour
{
    private ObjectDuplicator objectDuplicator; 
    public ObjectPositionData objectPositionData; 
    public GameObject duplicatedAvatar;
    private Vector3 player1position;
    GameObject OVRPlayerController;
    private float radius = 8f;  // 移動可能な半径を2mに設定

    void Start()
    {
        objectDuplicator = GameObject.Find("Player2 Room Copy").GetComponent<ObjectDuplicator>();
        duplicatedAvatar = GameObject.Find("Avatar1(Clone)");
        OVRPlayerController = GameObject.Find("OVRPlayerController");
    }

    void Update()
    {
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

            if (distanceFromCenter > radius)  // 半径を超えたら、可動範囲に戻す
            {
                Debug.Log($"AML-distanceFromCenter is:{distanceFromCenter}, radius is:{radius}");
                Vector3 direction = (player1position - avatarPosition).normalized;  // 円心からの方向ベクトル
                Debug.Log("AML-directon" + direction);
                player1position = objectDuplicator.duplicatedAvatar.transform.position + direction * radius;  // エッジに戻す
                Debug.Log("AML-corrected player1position:" + player1position);
                
                duplicatedAvatar.transform.position = player1position;
                OVRPlayerController.GetComponent<CharacterController>().enabled = false;
                OVRPlayerController.GetComponent<OVRPlayerController>().enabled = false;
                OVRPlayerController.transform.position = duplicatedAvatar.transform.position;
                OVRPlayerController.transform.rotation = duplicatedAvatar.transform.rotation;
                OVRPlayerController.GetComponent<CharacterController>().enabled = true;
                OVRPlayerController.GetComponent<OVRPlayerController>().enabled = true;
            }
        }
    }
}