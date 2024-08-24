using UnityEngine;
using UnityEngine.UI;

public class UIChanger : MonoBehaviour
{
    public static UIChanger Instance ;
    public Text treasureCountText;//unityeditorで設定するUIテキスト
        void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTreasureCount(int count)
    {
        //UIのテキストを更新する
        if(treasureCountText !=null)
        {
            treasureCountText.text="Treasures:"+count.ToString();
        }
        else
        {
            Debug.LogError("treasureCountTextが設定されていません");
        }

    }

}