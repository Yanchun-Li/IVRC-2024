using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using Oculus.Interaction;

public class PlayerSpecificWallInteraction : MonoBehaviour
{
    public string player1Tag = "Player1";
    public string movableWallTag = "Movable";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(player1Tag) && gameObject.CompareTag(movableWallTag))
        {
            // プレイヤー1が壁に触れたときの処理
            EnableWallInteraction(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(player1Tag) && gameObject.CompareTag(movableWallTag))
        {
            // プレイヤー1が壁から離れたときの処理
            EnableWallInteraction(false);
        }
    }

    private void EnableWallInteraction(bool enable)
    {
        // ここで壁の相互作用を有効/無効にする
        // 例: Interactable Unity Event Wrapperのコンポーネントを取得して制御
        InteractableUnityEventWrapper interactable = GetComponent<InteractableUnityEventWrapper>();
        if (interactable != null)
        {
            interactable.enabled = enable;
        }
    }

    // プレイヤーが壁を "消す" 操作を行ったときに呼び出すメソッド
    public void TryRemoveWall(GameObject player)
    {
        if (player.CompareTag(player1Tag) && gameObject.CompareTag(movableWallTag))
        {
            gameObject.SetActive(false);
        }
    }
}