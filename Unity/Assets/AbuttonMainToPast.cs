using UnityEngine;
using UnityEngine.UI;  
using Photon.Pun;

public class AbuttonMainToPast : MonoBehaviour
{
    public GameObject uiCanvas;
    public Button enterButton;  // 潜入するボタン
    public Button backButton;   // 戻るボタン
    private ObjectDuplicator objectDuplicator;
    private AccessCopyWorld accessCopyWorld;

    void Start()
    {
        // それぞれのスクリプトを持つオブジェクトを取得
        objectDuplicator = GameObject.FindObjectOfType<ObjectDuplicator>();
        accessCopyWorld = GameObject.FindObjectOfType<AccessCopyWorld>();

        // 潜入するボタンにリスナーを追加
        enterButton.onClick.AddListener(OnEnterButtonPressed);

        // 戻るボタンにリスナーを追加
        backButton.onClick.AddListener(OnBackButtonPressed);
    }

    void Update()
    {
        if(PhotonNetwork.NickName == "Player1")
        {
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                if (!uiCanvas.activeSelf)
                {
                    // Aボタンで遷移画面を表示
                    uiCanvas.SetActive(true);
                }
                else
                {
                    // 遷移画面が表示されている場合、Raycastでボタンを検出
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // VRの場合はコントローラのRayを使う
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Button buttonHit = hit.transform.GetComponent<Button>();
                        if (buttonHit != null)
                        {
                            if (buttonHit == enterButton)
                            {
                                // Rayが潜入ボタンにヒットしていて、Aボタンが押された場合
                                OnEnterButtonPressed();
                            }
                            else if (buttonHit == backButton)
                            {
                                // Rayが戻るボタンにヒットしていて、Aボタンが押された場合
                                OnBackButtonPressed();
                            }
                        }
                    }
                }
            }
        }

        else
        {
            //Player2がAボタンを押しても何も起こらない
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
            StartCoroutine(accessCopyWorld.Duration(5.0f));  // AccessCopyWorldの処理を実行
        }
    }

    void OnBackButtonPressed()
    {
        // 遷移画面が表示された後に、戻るボタンを押した時の処理
        uiCanvas.SetActive(false);  // 遷移画面を非表示にして、メイン画面に戻る
    }
}
