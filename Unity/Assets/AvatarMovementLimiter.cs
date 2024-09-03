using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class AvatarMovementLimiter : MonoBehaviour
{
    public ObjectDuplicator objectDuplicator; 
    public ObjectPositionData objectPositionData; 
    private Vector3 player1position;
    private float radius = 2f;  // 移動可能な半径を2mに設定

    void Start()
    {
        // 特別な初期化は不要
    }

    void Update()
    {
        if (objectDuplicator == null || objectPositionData == null)
        {
            return; // objectDuplicatorまたはobjectPositionDataがnullなら何もしない
        }

        if (objectDuplicator.duplicatedAvatar != null && objectPositionData.LengthPositions() > 0)
        {
            Vector3 avatarPosition = objectDuplicator.duplicatedAvatar.transform.position;  // duplicatedAvatarの現在位置を取得
            player1position = objectPositionData.GetPosition(objectPositionData.LengthPositions() - 1);  // 最新の位置を取得

            float distanceFromCenter = Vector3.Distance(player1position, avatarPosition);  // 円心からduplicatedAvatarまでの距離を計算

            if (distanceFromCenter > radius)  // 半径を超えたら、可動範囲に戻す
            {
                Vector3 direction = (avatarPosition - player1position).normalized;  // 円心からの方向ベクトル
                objectDuplicator.duplicatedAvatar.transform.position = player1position + direction * radius;  // エッジに戻す
            }
        }
    }
}