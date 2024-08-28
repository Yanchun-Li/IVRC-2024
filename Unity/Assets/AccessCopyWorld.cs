using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessCopyWorld : MonoBehaviour
{
    public bool accessOtherScene = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [SerializeField] public ObjectPositionData otherpositionData;
    [SerializeField] public ObjectRotationData otherrotationData;
    public List<int> indexlist; //共有を開始するインデックス、秒数ではないことに注意

    private int accessCount = 0;
    private Coroutine getPosition;
    ObjectDuplicator ObjectDuplicator;

    // Start is called before the first frame update
    void Start()
    {
        ObjectDuplicator = GameObject.Find("Player2 Room").GetComponent<ObjectDuplicator>();
        indexlist =  ObjectDuplicator.indexlist;  
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && !accessOtherScene)
        {
            originalPosition = this.transform.position;
            originalRotation = this.transform.rotation;

            if (accessOtherScene && getPosition != null){
                StopCoroutine(getPosition);
            }
            getPosition = StartCoroutine(Duration(5.0f));
        }
    }

    private IEnumerator Duration(float duration){
        accessOtherScene = true;
        float startTime = Time.time;
        Debug.Log("access copy world");

        Vector3 Position = otherpositionData.GetPosition(indexlist[accessCount]);
        Quaternion Rotation = otherrotationData.GetRotation(indexlist[accessCount]);
        this.transform.position = Position;
        this.transform.rotation = Rotation;
        if (indexlist[accessCount]>= otherpositionData.positions.Count){
                accessCount = 0;
                Debug.Log("currentIndex is clear");
        }
        while (Time.time - startTime < duration){
            yield return new WaitForSeconds(0.1f);
        }

        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
        accessCount++;
        accessOtherScene = false;
    }
}