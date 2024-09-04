using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceAndVectorToChest : MonoBehaviour
{
    private float searchRadius = 150f;
    public Text distanceText;   // 距離を表示するTextコンポーネント
    public GameObject arrowPrefab;    // 矢印UIのコンポーネント
    private ObjectDuplicator objectDuplicator; 
    public ObjectPositionData player1PositionData;  // Player1の位置データを取得
    private Transform nearestObject;
    private Vector3 moveDirection;
    public float arrowDistanceFromPlayer = 1f;  // 矢印がplayer1から1m離れて存在する距離

    void Update()
    {
        // objectDuplicator または player1PositionData が null なら処理をスキップ
        if (objectDuplicator == null || player1PositionData == null || objectDuplicator.duplicatedAvatar == null)
        {
            return;
        }

        // player1の位置をObjectPositionDataから取得
        if (player1PositionData.LengthPositions() > 0)
        {
            Vector3 player1Position = player1PositionData.GetPosition(player1PositionData.LengthPositions() - 1);
            
            // 最も近いオブジェクトを探す
            FindNearestObjectWithinRadius(player1Position);

            if (nearestObject != null)
            {
                // ターゲットから player1 へのベクトルを計算
                Vector3 directionFromTargetToPlayer = player1Position - nearestObject.position;
                directionFromTargetToPlayer.y = 0; // 水平方向のみを考慮する場合はY成分を無視

                // 距離を計算して表示
                float distanceToTarget = directionFromTargetToPlayer.magnitude;
                distanceText.text = "Distance: " + distanceToTarget.ToString("F0") + "m";

                // 矢印UIをプレイヤーを原点として回転
                float angle = Mathf.Atan2(directionFromTargetToPlayer.z, directionFromTargetToPlayer.x) * Mathf.Rad2Deg;
                arrowPrefab.transform.rotation = Quaternion.Euler(0, 0, angle); // 矢印の方向に合わせる

                // 矢印をplayer1の周囲1mの円上に配置
                Vector3 arrowPosition = player1Position + directionFromTargetToPlayer.normalized * arrowDistanceFromPlayer;
                arrowPrefab.transform.position = new Vector3(arrowPosition.x, arrowPosition.z, 0); // 2D UIの座標を調整
            }
            else
            {
                distanceText.text = "Distance: N/A";
            }
        }
    }

    void FindNearestObjectWithinRadius(Vector3 playerPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, searchRadius);
        float nearestDistance = Mathf.Infinity;
        nearestObject = null;

        foreach (Collider collider in hitColliders)
        {
            if (collider.transform != transform) // 自身を無視
            {
                float distance = Vector3.Distance(playerPosition, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = collider.transform;
                }
            }
        }
    }
}
