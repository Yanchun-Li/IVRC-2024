using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public List<Player> GetAllPlayers()
    {
        return new List<Player>(PhotonNetwork.PlayerList);
    }

    public Vector3 GetPlayerPosition(Player player)
    {
        Vector3 position = Vector3.zero;
        if (player.CustomProperties.TryGetValue("PosX", out object x) &&
            player.CustomProperties.TryGetValue("PosY", out object y) &&
            player.CustomProperties.TryGetValue("PosZ", out object z))
        {
            position = new Vector3((float)x, (float)y, (float)z);
        }
        return position;
    }

    public int GetPlayerScore(Player player)
    {
        int ans = 0;
        if (player.CustomProperties.TryGetValue("Score", out object score)){
            ans = (int) score;
        }
        return ans;
    }
}