using UnityEngine;
using Photon.Pun;

public class ObjectTransformSave : MonoBehaviourPunCallbacks
{
    public ObjectPositionData player1PositionData;
    public ObjectPositionData player2PositionData;
    public ObjectRotationData player1RotationData;
    public ObjectRotationData player2RotationData;

    private PhotonView photonView;

    //public bool isPlayer1;

    public float recordInterval = 0.1f;
    private float timer = 0.0f;
    private float currenttime=0.0f;
    private float passtime;

    private ObjectPositionData activePositionData;
    private ObjectRotationData activeRotationData;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            activePositionData = player1PositionData;
            activeRotationData = player1RotationData;
        }
        else
        {
            activePositionData = player2PositionData;
            activeRotationData = player2RotationData;
        }


    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > recordInterval)
        {
            activePositionData.AddPosition(transform.position);
            //オブジェクトの座標を表示
            Debug.Log(activePositionData + "Position:" + transform.position + "ID:" + photonView.OwnerActorNr);
            activePositionData.isUpdating = true;

            activeRotationData.AddRotation(transform.rotation);
            activeRotationData.isUpdating = true;

            passtime = Time.time - currenttime;
            //Debug.Log($"timer is {timer}, recordInterval is {recordInterval}");
            timer = 0f;
        }
        // positionData.AddPosition(transform.position);
        // positionData.isUpdating = true;
        currenttime = Time.time;
    }

    void OnDisable()
    {
        if (activePositionData != null)
            activePositionData.isUpdating = false;
        if (activeRotationData != null)
            activeRotationData.isUpdating = false;
    }
}