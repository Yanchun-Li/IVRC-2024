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
    GameObject UIChangerObject;
    private bool gameStart = false;
    private bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        playerlist = new List<Player>(PhotonNetwork.PlayerList);
        //playerの人数が二人になるまで、宝操作・座標保存・タイマー・介入を止めておく
        player1 = GameObject.Find("Avatar1(Clone)");
        player1.GetComponent<PlayerController>().enabled = false;
        player1.GetComponent<ChestRayInteraction>().enabled = false;
        //player1.GetComponent<OVRPlayerController>().enabled = false;
        player1.GetComponent<ObjectTransformSave>().enabled = false;
        GameManager = GameObject.Find("GameManager");
        GameManager.GetComponent<PlayerDataController>().enabled=false;
        timer = GameObject.Find("Avatar1(Clone)/Canvas/Timer");
        timer.SetActive(false);
        loading = GameObject.Find("Avatar1(Clone)/Canvas/Loading");
        loading.SetActive(true);
        UIChangerObject = GameObject.Find("UIChangerObject");
        UIChangerObject.GetComponent<AbuttonMainToPast>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);
        if (playerlist.Count == 2 & !gameStart){
            //player1.GetComponent<OVRPlayerController>().enabled = true;
            GameManager.GetComponent<PlayerDataController>().enabled=true;
            player1.GetComponent<PlayerController>().enabled = true;
            UIChangerObject.GetComponent<AbuttonMainToPast>().enabled = true;
            player1.GetComponent<ChestRayInteraction>().enabled = true;
            //player1.GetComponent<ObjectTransformSave>().enabled = true;
            StartCoroutine("GameStart");
        }
        // if (OVRInput.GetDown(OVRInput.Button.Two) & !gameStart){
        //     player1.GetComponent<OVRPlayerController>().enabled = true;
        //     player1.GetComponent<ObjectTransformSave>().enabled = true;
        //     StartCoroutine("GameStart");
        // }

        //状態の更新
        foreach (Player player in playerlist)
        {
            if (player.IsLocal){
                isPlaying = GetBool(player);//自分が介入中か
            }
        }

        //ゲーム開始かつプレイが終わったらいろいろ止める(UIChangerObjectのAbutton~とPlayer1のChestrayinteraction)
        if (gameStart == true && isPlaying == false){
            //player1.GetComponent<PlayerController>().enabled = false;
            UIChangerObject.GetComponent<AbuttonMainToPast>().enabled = false;
            player1.GetComponent<ChestRayInteraction>().enabled = false;
        }
    }

    IEnumerator GameStart(){
        gameStart = true;
        loading.GetComponent<Text>().text = "Game Start!!";
        yield return new WaitForSeconds(1.0f);
        loading.SetActive(false);
        timer.SetActive(true);
    }

    private bool GetBool(Player player){
        bool isPlaying = true;//適当な初期値
        if (player.CustomProperties.TryGetValue("isPlaying", out object playing)){
            isPlaying = (bool)playing;
        }
        return isPlaying;
    }
}
