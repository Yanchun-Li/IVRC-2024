using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class AccessShow : MonoBehaviourPunCallbacks
{
    private bool isAccess;
    [SerializeField] Text AccessText;
    // Start is called before the first frame update
    void Start()
    {
        AccessText.text = "";
        isAccess = false;
    }

    // Update is called once per frame
    void Update()
    {
        //状態の更新
        var playerlist = new List<Player>(PhotonNetwork.PlayerList);
        foreach (Player player in playerlist)
        {
            if (player.IsLocal){
                isAccess = GetPlayerAccess(player);//自分が介入中か
            }
        }

        if (isAccess){
            AccessText.text = "介入中です！消える壁を探し、消してください！";
        }else{
            AccessText.text = "";
        }
    }

    private bool GetPlayerAccess(Player player)
    {
        bool accesscheck = false;//エラー回避用に初期値false（player2はずっとTryGetValueができない→ずっとfalse）
        if (player.CustomProperties.TryGetValue("isAccessing", out object accessing))
        {
            accesscheck = (bool)accessing;
        }
        return accesscheck;
    }
}
