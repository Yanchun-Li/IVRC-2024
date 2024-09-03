using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransformData : MonoBehaviour
{
    public ObjectPositionData positionData1;
    public ObjectPositionData positionData2;
    public ObjectRotationData rotationData1;
    public ObjectRotationData rotationData2;
    // Start is called before the first frame update
    void Start()
    {
        positionData1.ClearPositions();
        positionData2.ClearPositions();
        rotationData1.ClearRotations();
        rotationData2.ClearRotations();
    }
}
