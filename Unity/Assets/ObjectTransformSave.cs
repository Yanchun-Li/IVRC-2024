using UnityEngine;

public class ObjectTransformSave : MonoBehaviour
{
    public ObjectPositionData positionData;
    public ObjectRotationData rotationData;
    public float recordInterval = 0.1f;
    private float timer = 0.0f;
    private float currenttime=0.0f;
    private float passtime;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > recordInterval){
            positionData.AddPosition(transform.position);
            positionData.isUpdating = true;
            rotationData.AddRotation(transform.rotation);
            rotationData.isUpdating = true;
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
        positionData.isUpdating = false;
        rotationData.isUpdating = false;
    }
}