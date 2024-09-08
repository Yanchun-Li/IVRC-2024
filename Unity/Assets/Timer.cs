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

    bool otherAccess = false;//相手がアクセスしてきたかどうか
    bool popup = false;//ポップアップの表示
    bool accessfinish = true;//一回のアクセスが終了したとき

    void Start(){
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isPlaying = true;
        if (PhotonNetwork.LocalPlayer.NickName == "Player1"){
                Timerspeed = 2;
        }else if (PhotonNetwork.NickName == "Player2"){
                Timerspeed = 1;
        }
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
                otherAccess = GetPlayerAccess(player);
            }else if (player.IsLocal){
                myscore = GameManager.GetPlayerScore(player);
                myPlaying = GetPlayerBool(player);
            }
            //Debug.Log($"my score is {myscore}, and othre score is {otherscore}");
        }

        if (otherAccess == true & accessfinish == true){
            popup = true;
            accessfinish = false;
            StartCoroutine(PopupWindow(timeLimit, (int)realtime));
        }

        //timerTextを更新していく（他人が介入していないとき）
        if (popup == false){
            if (remaining > 0){
                //残り時間がある場合
                foreach(Player player in playerlist)
                if(player.NickName =="Player1")
                    {
                        timerText.text=$"残り時間：{remaining.ToString("D3")}秒\nAボタンで過去に遷移\n自分のスコア：{myscore}\n相手のスコア：{otherscore}";
                    }
                else
                    {
                        timerText.text=$"残り時間：{remaining.ToString("D3")}秒\n自分のスコア：{myscore}\n相手のスコア：{otherscore}";
                    }
                
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
        }
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

    private bool GetPlayerAccess(Player player)
    {
        bool accesscheck=false;//エラー回避用に初期値false（player2はずっとTryGetValueができない→ずっとfalse）
        if (player.CustomProperties.TryGetValue("isAccessing", out object accessing)){
            accesscheck = (bool) accessing;
        }
        return accesscheck;
    }

    private System.Collections.IEnumerator PopupWindow(int timelimit, int realtime)
    {
        int updatetime = timelimit - realtime*2;//スピードを2で固定（参照無理だった・・・）
        timerText.text = $"相手がこちらの世界に介入しました\nこちらの世界に反映されるのは残り時間{updatetime}秒の時です\n※このウィンドウは自動的に消滅します";
        yield return new WaitForSeconds(3.0f);
        popup = false;
        while(otherAccess==true){
            var playerlist = new List<Player>(PhotonNetwork.PlayerList);
            foreach (Player player in playerlist){
                if (!player.IsLocal){
                    otherAccess = GetPlayerAccess(player);
                }
            }
        }
        //アクセスが終了したらtrueに戻す
        accessfinish = true;
    }

}