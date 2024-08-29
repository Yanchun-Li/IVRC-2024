using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerStop : MonoBehaviourPunCallbacks
{
    private List<Player> playerlist;
    GameObject player1;
    GameObject timer;
    GameObject loading;
    GameObject GameManager;
    private bool gameStart = false;

    // Start is called before the first frame update
    void Start()
    {
        playerlist = new List<Player>(PhotonNetwork.PlayerList);
        //playerの人数が二人になるまで、移動・座標保存・タイマーを止めておく
        player1 = GameObject.Find("Avatar1(Clone)");
        player1.GetComponent<OVRPlayerController>().enabled = false;
        player1.GetComponent<ObjectTransformSave>().enabled = false;
        GameManager = GameObject.Find("GameManager");
        GameManager.GetComponent<PlayerDataController>().enabled=false;
        timer = GameObject.Find("Avatar1(Clone)/Canvas/Timer");
        timer.SetActive(false);
        loading = GameObject.Find("Avatar1(Clone)/Canvas/Loading");
        loading.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        playerlist = new List<Player>(PhotonNetwork.PlayerList);
        if (playerlist.Count == 2 & !gameStart){
            player1.GetComponent<OVRPlayerController>().enabled = true;
            GameManager.GetComponent<PlayerDataController>().enabled=true;
            //player1.GetComponent<ObjectTransformSave>().enabled = true;
            StartCoroutine("GameStart");
        }
        // if (OVRInput.GetDown(OVRInput.Button.Two) & !gameStart){
        //     player1.GetComponent<OVRPlayerController>().enabled = true;
        //     player1.GetComponent<ObjectTransformSave>().enabled = true;
        //     StartCoroutine("GameStart");
        // }
    }

    IEnumerator GameStart(){
        gameStart = true;
        loading.GetComponent<Text>().text = "Game Start!!";
        yield return new WaitForSeconds(1.0f);
        loading.SetActive(false);
        timer.SetActive(true);
    }
}
