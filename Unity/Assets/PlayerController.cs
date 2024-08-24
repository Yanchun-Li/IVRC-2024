using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView photonView;
    private ChestRayInteraction chestRayInteraction;
    private int score = 0;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        chestRayInteraction = GetComponent<ChestRayInteraction>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // ローカルプレイヤーの移動処理
            // 移動後に位置情報を更新
            UpdatePlayerPositionandScore();
        }
    }

    void UpdatePlayerPositionandScore()
    {
        Vector3 position = transform.position;
        int score = chestRayInteraction.GetScore();
        Hashtable hash = new Hashtable();
        hash.Add("PosX", position.x);
        hash.Add("PosY", position.y);
        hash.Add("PosZ", position.z);
        hash.Add("Score", score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(score);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            score = (int)stream.ReceiveNext();
        }
    }
}