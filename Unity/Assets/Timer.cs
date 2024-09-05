using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviourPunCallbacks
{
    // 制限時間
    [SerializeField] int timeLimit;
    //タイマー用テキスト
    [SerializeField] Text timerText;
    //経過時間
    public float realtime;
    [SerializeField] GameObject Camera;
    public int Timerspeed;
    GameManager GameManager;
    int myscore;
    int otherscore;
    int maxscore = 20;
    bool isPlaying=false;//プレイ中か判定するブール
    bool myPlaying;
    bool otherPlaying;

    void Start(){
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isPlaying = true;
    }

    void Update()
    {
        //フレーム毎の経過時間をtime変数に追加
        realtime += Time.deltaTime*Timerspeed;
        //time変数をint型にし制限時間から引いた数をint型のlimit変数に代入
        int remaining = timeLimit -(int)realtime;
        
        //boolの更新
        if (remaining < 0){
            isPlaying = false;
        }
        UpdateBool(isPlaying);

        //状態の更新
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);
        foreach (Player player in playerlist){
            if (!player.IsLocal){
                otherscore = GameManager.GetPlayerScore(player);
                otherPlaying = GetPlayerBool(player);
            }else if (player.IsLocal){
                myscore = GameManager.GetPlayerScore(player);
                myPlaying = GetPlayerBool(player);
            }
            //Debug.Log($"my score is {myscore}, and othre score is {otherscore}");
        }


        //timerTextを更新していく
        if (remaining > 0){
            //残り時間がある場合
            timerText.text=$"残り時間：{remaining.ToString("D3")}秒\n自分のスコア：{myscore}\n相手のスコア：{otherscore}";
            isPlaying = true;
        }else if(otherPlaying == true){
            //player2を待つ状態
            timerText.text="ゲーム終了！\nもう一人のプレイヤーが終了するまでお待ちください";
        }else{
            //両方終了した状態
            int totalscore = myscore + otherscore;
            if (totalscore < maxscore*0.7f){
                timerText.text=$"お疲れさまでした！\nお二人の点数は{totalscore}/{maxscore}です！";
            }else if(totalscore < maxscore){
                timerText.text=$"お疲れさまでした！\nお二人の点数は{totalscore}/{maxscore}です！\nほぼすべての宝を獲得しましたね";
            }else{
                 timerText.text=$"お疲れさまでした！\nお二人の点数は{totalscore}/{maxscore}です！\n満点です、お見事！！";
            }
        }
       // Debug.Log($"camera position is {Camera.transform.position} and position is {this.transform.position}");
    }

    private void UpdateBool(bool playing)
    {
        ExitGames.Client.Photon.Hashtable play = new ExitGames.Client.Photon.Hashtable() { { "isPlaying", playing } };
        try
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(play);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error setting custom properties: {e.Message}");
        }

        StartCoroutine(CheckScoreAfterUpdate());
    }

    private System.Collections.IEnumerator CheckScoreAfterUpdate()
    {
        yield return new WaitForSeconds(0.1f);//高速で更新しすぎるのを防ぐ
    }

    private bool GetPlayerBool(Player player)
    {
        bool playcheck=false;//エラー回避用に初期値false
        if (player.CustomProperties.TryGetValue("isPlaying", out object play)){
            playcheck = (bool) play;
        }
        return playcheck;
    }

}