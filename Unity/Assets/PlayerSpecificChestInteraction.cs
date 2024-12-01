using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using Oculus.Interaction;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpecificChestInteraction : MonoBehaviourPunCallbacks
{
    public string player1Tag = "Player1";
    public GameObject gameObject;
    private PhotonView photonView;
    public float positionX;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        positionX = gameObject.transform.position.x;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(player1Tag) && positionX > -100)
        {
           if (photonView.IsMine)
            {
                photonView.RPC("EnableWallInteractionRPC", RpcTarget.All, true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(player1Tag) && gameObject.CompareTag(movableWallTag))
        {
            // プレイヤー1が壁から離れたときの処理
           if (photonView.IsMine)
            {
                photonView.RPC("EnableWallInteraction", RpcTarget.All, false);
            }
        }
    }

    [PunRPC]
    public void EnableWallInteraction(bool enable)
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
    public void TryRemoveWall()
    {
        Player player = PhotonNetwork.LocalPlayer;
        if (player.NickName == player1Tag && gameObject.CompareTag(movableWallTag))
        {
             if (photonView.IsMine)
            {
                photonView.RPC("RemoveWallRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void RemoveWallRPC()
    {
        gameObject.SetActive(false);
        Debug.Log("wall is deleted");
    }
}