using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class AvatarMovementLimiter : MonoBehaviour
{
    public ObjectDuplicator objectDuplicator; 
    public Transform playerTransform; 
    private Vector3 circleCenter;
    private float radius = 2f;  // 移動可能な半径を2mに設定

    void Start()
    {
        // 特別な初期化は不要
    }

    void Update()
    {
        if (objectDuplicator == null || playerTransform == null)
        {
            return; // objectDuplicatorやplayerTransformがnullなら何もしない
        }

        if (objectDuplicator.duplicatedAvatar != null)
        {
            Vector3 avatarPosition = objectDuplicator.duplicatedAvatar.transform.position;  // duplicatedAvatarの現在位置を取得
            circleCenter = playerTransform.position;  // 可動範囲の円心を更新

            float distanceFromCenter = Vector3.Distance(circleCenter, avatarPosition);  // 円心からduplicatedAvatarまでの距離を計算

            if (distanceFromCenter > radius)  // 半径を超えたら、可動範囲に戻す
            {
                Vector3 direction = (avatarPosition - circleCenter).normalized;  // 円心からの方向ベクトル
                objectDuplicator.duplicatedAvatar.transform.position = circleCenter + direction * radius;  // エッジに戻す
            }
        }
    }
}