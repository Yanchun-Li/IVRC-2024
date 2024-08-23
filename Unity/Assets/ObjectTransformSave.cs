using UnityEngine;
using Photon.Pun;

public class ObjectTransformSave : MonoBehaviourPunCallbacks
{
    public ObjectPositionData player1PositionData;
    public ObjectPositionData player2PositionData;
    public ObjectRotationData player1RotationData;
    public ObjectRotationData player2RotationData;

    public float recordInterval = 0.1f;
    private float timer = 0.0f;
    private float currenttime=0.0f;
    private float passtime;

    private ObjectPositionData activePositionData;
    private ObjectRotationData activeRotationData;

    void Awake()
    {
        Debug.Log("Awake内のOwnerActorNr: " + photonView.OwnerActorNr);

    }

    void Start()
    {

         Debug.Log("OwnerActorNr: " + photonView.OwnerActorNr);

        if (photonView.OwnerActorNr == 0)
        {
            activePositionData = player1PositionData;
            activeRotationData = player1RotationData;
        }
        if(photonView.OwnerActorNr==1)
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