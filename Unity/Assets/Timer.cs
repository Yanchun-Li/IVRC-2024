using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    // 制限時間
    [SerializeField] int timeLimit;
    //タイマー用テキスト
    [SerializeField] Text timerText;
    //経過時間
    float time;
    [SerializeField] GameObject Camera;
    void Update()
    {
        //フレーム毎の経過時間をtime変数に追加
        time += Time.deltaTime;
        //time変数をint型にし制限時間から引いた数をint型のlimit変数に代入
        int remaining = timeLimit -(int)time;
        //timerTextを更新していく
        timerText.text=$"残り：{remaining.ToString("D3")}";
        Debug.Log($"camera position is {Camera.transform.position} and position is {this.transform.position}");
    }
}