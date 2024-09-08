using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreatePhotonAvatar : MonoBehaviourPunCallbacks
{ 
    private GameObject masterPlayerObject;
    private GameObject[] rootTargets = new GameObject[5];

    [SerializeField] GameObject[] Targets = new GameObject[5];

    private bool isCreated = false;

    private void OnCreate()
    {
        masterPlayerObject = GameObject.FindGameObjectWithTag("MasterPlayer");
        rootTargets[0] = GameObject.FindGameObjectWithTag("CameraRig");
        rootTargets[1] = GameObject.FindGameObjectWithTag("TrackingSpace");
        rootTargets[2] = GameObject.FindGameObjectWithTag("MainCamera");
        rootTargets[3] = GameObject.FindGameObjectWithTag("LHandTargetAnchor");
        rootTargets[4] = GameObject.FindGameObjectWithTag("RHandTargetAnchor");
        for(int i = 0; i < 5; i++)
        {
            if(rootTargets[i] == null)
            {
                Debug.Log(i);
                Debug.LogError("Target is not found");
            }
        }

        isCreated = true; 
    }

    private void OnEnable()
    {
        if(photonView.IsMine)
        {
            OnCreate();
        }
    }

    void Update()
    {
        if(isCreated)
        {
            this.transform.position = masterPlayerObject.transform.position;
            this.transform.rotation = masterPlayerObject.transform.rotation;

            for(int i = 0; i < 5; i++)
            {
                Targets[i].transform.localPosition = rootTargets[i].transform.localPosition;
                Targets[i].transform.localRotation = rootTargets[i].transform.localRotation;
            }
            //Debug.Log($"name {masterPlayerObject.name} and position {masterPlayerObject.transform.position}");

        }
    }
}

