using UnityEngine;
using Photon.Pun;

public class ObjectTransformSave : MonoBehaviour
{
    public ObjectPositionData player1PositionData;
    public ObjectPositionData player2PositionData;
    public ObjectRotationData player1RotationData;
    public ObjectRotationData player2RotationData;

    private ObjectPositionData activePositionData;
    private ObjectRotationData activeRotationData;
    public float recordInterval = 0.1f;
    private float timer = 0.0f;
    private float currenttime=0.0f;
    private float passtime;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
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
        if (timer > recordInterval){
            // positionData.AddPosition(transform.position);
            // positionData.isUpdating = true;
            // rotationData.AddRotation(transform.rotation);
            // rotationData.isUpdating = true;
            // passtime = Time.time - currenttime;
            activePositionData.AddPosition(transform.position);
            activeRotationData.AddRotation(transform.rotation);
            //Debug.Log($"timer is {timer}, recordInterval is {recordInterval}");
            timer = 0f;
        }
        // positionData.AddPosition(transform.position);
        // positionData.isUpdating = true;
        // currenttime = Time.time;
    }

    void OnDisable()
    {
        // positionData.isUpdating = false;
        // rotationData.isUpdating = false;
    }
}