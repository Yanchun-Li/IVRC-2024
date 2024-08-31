using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SetPosition : MonoBehaviourPunCallbacks
{
    private Vector3 startposition1 = new Vector3(0f,0f,0f);
    private Vector3 startposition2 = new Vector3(200f,0f,0f);
    private Vector3 position;
    private bool avatarname = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NickName == "Player1"){
                position = startposition1;
                avatarname = true;
        }else if (PhotonNetwork.NickName == "Player2"){
                position = startposition2;
                avatarname = true;
        }
        this.transform.position += position;
        Debug.Log(PhotonNetwork.NickName);
    }

    // Update is called once per frame
    void Update()
    {
        if(avatarname == false){
            if (PhotonNetwork.NickName == "Player1"){
                position = startposition1;
                avatarname = true;
                Debug.Log(PhotonNetwork.NickName);
            }else if (PhotonNetwork.NickName == "Player2"){
                position = startposition2;
                avatarname = true;
            }
        }
    }
}