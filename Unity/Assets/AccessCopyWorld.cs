using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AccessCopyWorld : MonoBehaviour
{
    public bool accessOtherScene = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [SerializeField] public ObjectPositionData otherpositionData;
    [SerializeField] public ObjectRotationData otherrotationData;
    // public List<int> indexlist; //共有を開始するインデックス、秒数ではないことに注意

    private int accessCount = 0;
    private Coroutine getPosition;
    ObjectDuplicator ObjectDuplicator;

    // Start is called before the first frame update
    void Start()
    {
        ObjectDuplicator = GameObject.Find("Player2 Room Copy").GetComponent<ObjectDuplicator>();
        // indexlist =  ObjectDuplicator.indexlist; 
        UpdateBool(accessOtherScene); 
    }

    // Update is called once per frame
    void Update()
    {
        // indexlist =  ObjectDuplicator.indexlist; 
        if (OVRInput.GetDown(OVRInput.Button.One) && !accessOtherScene)
        {
            originalPosition = this.transform.position;
            originalRotation = this.transform.rotation;

            // if (accessOtherScene && getPosition != null){
            //     StopCoroutine(getPosition);
            // }
            // getPosition = StartCoroutine(Duration(10.0f));
        }
    }

    public IEnumerator Duration(float duration, int startindex){

        accessOtherScene = true;
        UpdateBool(accessOtherScene);
        float pasttime = Time.time;
        Debug.Log("access player2 world");
        Debug.Log($"Start index is:{startindex}");
        Vector3 Position = otherpositionData.GetPosition(startindex);
        Quaternion Rotation = otherrotationData.GetRotation(startindex);
        Vector3 difforigin = ObjectDuplicator.difforigin;
        this.transform.position = Position + difforigin;
        this.transform.rotation = Rotation;
        Debug.Log("get position and rotation");
        Debug.Log("diff origin in access copyworld" + difforigin);
        Debug.Log($"Receiving Position{Position}, Rotation{Rotation}");
        if (startindex >= otherpositionData.positions.Count){
                accessCount = 0;
                Debug.Log("currentIndex is clear");
        }
        while (Time.time - pasttime < duration){
            yield return new WaitForSeconds(0.1f);
        }

        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
        Debug.Log($"Resetted Position{Position}, Rotation{Rotation}");
        accessCount++;
        accessOtherScene = false;
        UpdateBool(accessOtherScene);
    }

    private void UpdateBool(bool accessing)
    {
        ExitGames.Client.Photon.Hashtable access = new ExitGames.Client.Photon.Hashtable() { { "isAccessing", accessing } };
        try
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(access);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error setting custom properties: {e.Message}");
        }
    }
}
