using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AddTreasure : MonoBehaviour
{
    public static AddTreasure Instance;
    private PhotonView photonView;
    private int totalTreasures = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        photonView = GetComponent<PhotonView>();
    }

    public void IncrermentTreasureCount()
    {
        totalTreasures++;
        UpdateUI();
    }

    void UpdateUI()
    {
        //両プレイヤーのUIを更新
        UIChanger.Instance.UpdateTreasureCount(totalTreasures);
    }
    
}
