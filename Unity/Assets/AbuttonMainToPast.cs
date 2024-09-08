using UnityEngine;
using UnityEngine.UI;  
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;

public class AbuttonMainToPast : MonoBehaviourPunCallbacks
{
    public GameObject uiCanvas;
    public List<Player> playerlist;
    private ObjectDuplicator objectDuplicator;
    private AccessCopyWorld accessCopyWorld;
    public Slider slider;       // スライダーの参照
    public float pasttime;
    [SerializeField ]private int startindex;

    void Start()
    {
        // それぞれのスクリプトを持つオブジェクトを取得
        objectDuplicator = GameObject.FindObjectOfType<ObjectDuplicator>();
        accessCopyWorld = GameObject.FindObjectOfType<AccessCopyWorld>();

        // 経過時間を初期化
        pasttime = 0f;
    }

void Update()
{   
    startindex = objectDuplicator.startindex;
    playerlist = new List<Player>(PhotonNetwork.PlayerList);
    if (playerlist.Count == 2)
    {
        pasttime += Time.deltaTime;
    }
    Debug.Log($"ACCESS COPY WORLD IS: {accessCopyWorld} (Before)");
    if (accessCopyWorld == null){
        accessCopyWorld = GameObject.FindObjectOfType<AccessCopyWorld>();
        Debug.Log($"ACCESS COPY WORLD IS: {accessCopyWorld} (After)");
        Debug.Log("Find Object of Type###################");

    }

    if (PhotonNetwork.NickName == "Player1")
    {
        // Aボタンで遷移画面を表示
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) && !uiCanvas.activeSelf)
        {
            uiCanvas.SetActive(true);
            Debug.Log("Aボタンを押して遷移画面が開きました");
        }
        // UIがアクティブな時にボタン操作を受け付ける
        else if (uiCanvas.activeSelf)
        {
            
            // Aボタン：潜入する
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                OnEnterButtonPressed();
                Debug.Log("Aボタンが押されました");
            }

            // Bボタン：戻る
            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                OnBackButtonPressed();
                Debug.Log("Bボタンが押されました");
            }

            // Yボタン：スライダーの値を10増やす
        // if (Input.GetKey(KeyCode.JoystickButton3))  // ボタンが押され続けている間、値を変更
        // {
        //     Debug.Log("Yボタンが押されています");
        //     if (slider != null)
        //     {
        //         slider.value = Mathf.Min(slider.value + 30 * Time.deltaTime, slider.maxValue);  // スライダーの値が300を超えないように
        //     }
        // }

        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch))  // ボタンが押され続けている間、値を変更
        {
            Debug.Log("Yボタンが押されています");
            if (slider != null)
            {
                slider.value = Mathf.Min(slider.value + 100 * Time.deltaTime , slider.maxValue);  // スライダーの値が300を超えないように
                slider.value = Mathf.Min(slider.value, pasttime);
            }
            Debug.Log("Slider Value is: " + slider.value);  
        }

        // Xボタン：スライダーの値を10減らす
        // if (Input.GetKey(KeyCode.JoystickButton2))  // ボタンが押され続けている間、値を変更
        // {
        //     Debug.Log("Xボタンが押されています");
        //     if (slider != null)
        //     {
        //         slider.value = Mathf.Max(slider.value - 30 * Time.deltaTime, slider.minValue);  // スライダーの値が0を下回らないように
        //     }
        // }

        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch))  // ボタンが押され続けている間、値を変更
        {
            Debug.Log("Xボタンが押されています");
            if (slider != null)
            {
                slider.value = Mathf.Max(slider.value - 100 * Time.deltaTime, slider.minValue);  // スライダーの値が0を下回らないように
                slider.value = Mathf.Min(slider.value, pasttime);
            }
            Debug.Log("Slider Value is: " + slider.value);  
        }
        }
    }
    else
    {
        // Player2がAボタンを押しても何も起こらない
    }
}

// ハンドルの位置を更新するメソッド
void UpdateHandlePosition(float sliderValue)
{
    // スライダーの範囲に基づいてハンドルの位置を調整
    RectTransform handleRectTransform = slider.handleRect;

    if (handleRectTransform != null)
    {
        float normalizedValue = (sliderValue - slider.minValue) / (slider.maxValue - slider.minValue);  // 正規化されたスライダーの値
        Vector2 newHandlePosition = new Vector2(normalizedValue * handleRectTransform.rect.width, handleRectTransform.anchoredPosition.y);
        handleRectTransform.anchoredPosition = newHandlePosition;
    }
}


    void OnEnterButtonPressed()
    {
        // 遷移画面が表示された後に、潜入ボタンを押した時の処理
        uiCanvas.SetActive(false);  // 遷移画面を非表示にする
        if (objectDuplicator != null)
        {
            objectDuplicator.DuplicateAndMove();  // ObjectDuplicatorの処理を実行
        }
        if (accessCopyWorld != null)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!! ACCESS COPY WORLD == TRUE ####################");
            StartCoroutine(accessCopyWorld.Duration(5.0f, startindex));  // AccessCopyWorldの処理を実行
        }
    }

    void OnBackButtonPressed()
    {
        // 遷移画面が表示された後に、戻るボタンを押した時の処理
        uiCanvas.SetActive(false);  // 遷移画面を非表示にして、メイン画面に戻る
    }
}
