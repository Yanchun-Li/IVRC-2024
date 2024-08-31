using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SetPosition : MonoBehaviourPunCallbacks
{
    private Vector3 startposition1 = new Vector3(0f,0f,0f);
    private Vector3 startposition2 = new Vector3(10f,0f,0f);
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NickName == "Player1"){
                position = startposition1;
            }else{
                position = startposition2;
            }
        this.transform.position += position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
