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
            return; // objectDuplicatorまたはobjectPositionDataがnullなら何もしない
        }
        
        if (objectDuplicator.duplicatedAvatar != null && objectPositionData.LengthPositions() > 0)
        {
            Vector3 avatarPosition = objectDuplicator.duplicatedAvatar.transform.position;  // duplicatedAvatarの現在位置を取得
            player1position = objectPositionData.GetPosition(objectPositionData.LengthPositions() - 1);  // 最新の位置を取得
            float distanceFromCenter = Vector3.Distance(player1position, avatarPosition);  // 円心からduplicatedAvatarまでの距離を計算
            Vector3 direction = (player1position - avatarPosition).normalized;  // 円心からの方向ベクトル
            
            if(distanceFromCenter > radius){
                player1position = objectDuplicator.duplicatedAvatar.transform.position + direction * 0.125f * radius;  // エッジに戻す
            }
        }
    }
}