using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Oculus.Platform.Models;

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
    GameObject OVRPlayerController;

    // Start is called before the first frame update
    void Start()
    {
        ObjectDuplicator = GameObject.Find("Player2 Room Copy").GetComponent<ObjectDuplicator>();
        OVRPlayerController = GameObject.Find("OVRPlayerController");
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
            Debug.Log("Original Position is" + originalRotation);

            // if (accessOtherScene && getPosition != null){
            //     StopCoroutine(getPosition);
            // }
            // getPosition = StartCoroutine(Duration(10.0f));
        }
    }

    public IEnumerator Duration(float duration, int startindex){
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        OVRPlayerController.GetComponent<CharacterController>().enabled = false;
        OVRPlayerController.GetComponent<OVRPlayerController>().enabled = false;

        accessOtherScene = true;
        UpdateBool(accessOtherScene);
        float pasttime = Time.time;

        Debug.Log("access player2 world");
        // Debug.Log($"Start index in accesscopyworld is:{startindex}");
        // Debug.Log($"LengthPosition in accesscopyworld is:{otherpositionData.LengthPositions()}");
        // Debug.Log($"LengthPosition in accesscopyworld is:{otherrotationData.LengthRotations()}");
        if (startindex > otherpositionData.LengthPositions()){ Debug.LogError($"Start index is {startindex}, len is {otherpositionData.LengthPositions()}");}
        Vector3 Position = otherpositionData.GetPosition(startindex - 1);
        Quaternion Rotation = otherrotationData.GetRotation(startindex - 1);
        Vector3 difforigin = ObjectDuplicator.difforigin;
        this.transform.position = Position + difforigin;
        this.transform.rotation = Rotation;
        OVRPlayerController.transform.position = Position + difforigin;;
        OVRPlayerController.transform.rotation = Rotation;
        Debug.Log("get position and rotation");
        Debug.Log("diff origin in access copyworld" + difforigin);
        Debug.Log($"Receiving Position{Position}");

        OVRPlayerController.GetComponent<CharacterController>().enabled = true;
        OVRPlayerController.GetComponent<OVRPlayerController>().enabled = true;

        if (startindex >= otherpositionData.positions.Count){
                accessCount = 0;
                Debug.Log("currentIndex is clear");
        }
        while (Time.time - pasttime < duration){
            yield return new WaitForSeconds(0.1f);
        }

        OVRPlayerController.GetComponent<CharacterController>().enabled = false;
        OVRPlayerController.GetComponent<OVRPlayerController>().enabled = false;
        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
        OVRPlayerController.transform.position = originalPosition;
        OVRPlayerController.transform.rotation = originalRotation;
        
        Debug.Log($"Resetted Position{originalPosition}");
        OVRPlayerController.GetComponent<CharacterController>().enabled = true;
        OVRPlayerController.GetComponent<OVRPlayerController>().enabled = true;

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
