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
    [SerializeField] int Timerspeed;
    GameManager GameManager;
    int myscore;
    int otherscore;
     [SerializeField] GameObject endPanel;
    void Start(){
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        endPanel.SetActive(false);
    }

    void Update()
    {
        //フレーム毎の経過時間をtime変数に追加
        realtime += Time.deltaTime*Timerspeed;
        //time変数をint型にし制限時間から引いた数をint型のlimit変数に代入
        int remaining = timeLimit -(int)realtime;
        //スコアの取得
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);
        foreach (Player player in playerlist){
            if (!player.IsLocal){
                otherscore = GameManager.GetPlayerScore(player);
            }else if (player.IsLocal){
                myscore = GameManager.GetPlayerScore(player);
            }
            //Debug.Log($"my score is {myscore}, and othre score is {otherscore}");
        }
        //timerTextを更新していく
        timerText.text=$"残り時間：{remaining.ToString("D3")}秒\n自分のスコア：{myscore}\n相手のスコア：{otherscore}";
        if (remaining <= 0)
        {
            EndGame();
        }
       // Debug.Log($"camera position is {Camera.transform.position} and position is {this.transform.position}");
    }
    void EndGame()
    {
        // 終了画面を表示する
        endPanel.SetActive(true);
        // 終了画面のテキストを設定する
        Text endPanelText = endPanel.GetComponentInChildren<Text>(); // 子要素にTextコンポーネントを持つ要素が必要
        if (endPanelText != null)
        {
            endPanelText.text = $"Final Score: {myscore}";
        }
        // ゲームの更新を止める
        Time.timeScale = 0f;
    }
}