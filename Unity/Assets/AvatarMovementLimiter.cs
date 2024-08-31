using UnityEngine;

public class AvatarMovementLimiter : MonoBehaviour
{
    public ObjectDuplicator objectDuplicator; 
    public Transform playerTransform; 
    private Vector3 circleCenter;
    private float radius = 2f;                 // 移動可能な半径を２ｍにする

    void Start()
    {
        if (objectDuplicator != null && objectDuplicator.duplicatedAvatar != null)
        {
            circleCenter = objectDuplicator.duplicatedAvatar.transform.position;
        }
    }

    void Update()
    {
        if (objectDuplicator != null && objectDuplicator.duplicatedAvatar != null)
        {
            Debug.Log("duplicatedAvatar is created");
            Vector3 avatarPosition = objectDuplicator.duplicatedAvatar.transform.position;  //　duplicatedAvatarの現在位置
            circleCenter = playerTransform.position;                                        //　可動範囲の円心を更新

            float distanceFromCenter = Vector3.Distance(circleCenter, avatarPosition);      //　円心からduplicatedAvatarまでの距離を計算

            if (distanceFromCenter > radius)                                                //　半径を越えたら，可動範囲に戻される
            {
                Vector3 direction = (avatarPosition - circleCenter).normalized;             //　円心からの方向ベクトル
                objectDuplicator.duplicatedAvatar.transform.position = circleCenter + direction * radius;   //　エッジに戻る
            }
        }
    }
}
