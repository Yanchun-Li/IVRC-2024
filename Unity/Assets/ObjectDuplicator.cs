using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ObjectDuplicator : MonoBehaviourPunCallbacks
{
    public GameObject originalObject;  // 元のオブジェクト
    public GameObject originalAvatar;  // 元のアバター
    public GameObject copyAvatar;      // コピーアバター
    public ObjectPositionData positionData;  // 位置データの格納
    public ObjectRotationData rotationData;  // 回転データの格納
    public Vector3 newPosition;  // 新しい位置
    public float duration = 10f;  // 持続時間
    public Slider slider;         // 時間調整用のスライダー

    private float updatetime; //更新する時間
    private int accessCount = 0;  // アクセス回数
    public int startindex = 0;    // 開始インデックス
    public Vector3 difforigin = new Vector3(0.0f, 0.0f, 0.0f); // 原点からのずれ

    private bool isProcessing = false;  // 重複操作を防ぐフラグ
    private float moveSpeed = 0.05f;    // 移動速度
    private float pasttime = 0.0f;       // 経過時間

    // オブジェクトプール部分
    private List<GameObject> avatarPool = new List<GameObject>();  // アバター用のオブジェクトプール
    private int poolSize = 10;  // プールの最大サイズを10に制限

    public GameObject duplicatedAvatar;  // 複製されたアバター
    public GameObject duplicatedObject;
    private PhotonView photonView;

    private void Start()
    {
        newPosition = this.transform.position;  // 初期位置を設定
    }

    private void Update()
    {
        if (positionData.LengthPositions() > 10)
        {
            // 位置データに基づいて原点のずれを計算
            difforigin = newPosition - positionData.GetPosition(10);
            difforigin.y = 0;  // Y軸方向は変えない
            pasttime += Time.deltaTime;  // 経過時間を記録
        }

        // ボタンが押され、かつ処理中でない場合
        if (OVRInput.GetDown(OVRInput.Button.One) && !isProcessing)
        {
            updatetime = pasttime * 2;
            //DuplicateAndMove();  // 複製と移動を実行（別のスクリプトで呼び出すのでここでは呼ばない）
        }

        if (duplicatedAvatar != null)
        {
            SmoothMove();  // アバターのスムーズな移動を実行
        }
    }

    // オブジェクトプールからアバターを取得。プールに空きがなければ新しいオブジェクトを作成
    private GameObject GetPooledAvatar()
    {
        foreach (var avatar in avatarPool)
        {
            if (!avatar.activeInHierarchy)
            {
                return avatar;
            }
        }

        // プールに空きがあり、新しいオブジェクトが作成可能な場合
        if (avatarPool.Count < poolSize)
        {
            GameObject newAvatar = Instantiate(copyAvatar, newPosition, Quaternion.identity);
            avatarPool.Add(newAvatar);
            return newAvatar;
        }

        return null;  // プールが満杯の場合、新しいオブジェクトは作成しない
    }

    // オブジェクトを複製して移動させる
    public void DuplicateAndMove()
    {
        if (isProcessing) return;  // 処理中の場合、操作を無視

        isProcessing = true;
        duplicatedAvatar = GetPooledAvatar();  // プールからアバターを取得

        if (duplicatedAvatar != null)
        {
            Debug.Log("create duplicateAvatar");
            duplicatedAvatar.SetActive(true);  // オブジェクトを有効化
            StartCoroutine(UpdateAndDestroy());  // 更新と削除の処理を開始
            StartCoroutine(UpdateAvatarPosition());  // アバターの位置更新を開始
        }
    }

    // コルーチン：一定時間後にオブジェクトを非表示にする
    private IEnumerator UpdateAndDestroy()
    {
        Debug.Log($"start UpdateAndDestroy Coroutine, duration is {duration}, pasttime is {pasttime}, updatetime is {updatetime}");
        yield return new WaitForSeconds(duration);
        duplicatedAvatar.SetActive(false);  // アバターを無効化してプールに戻す
        yield return new WaitUntil(() => pasttime >= updatetime);
        Debug.Log("start UpdateOrignalObject Coroutine");
        UpdateOriginalObject(originalObject, duplicatedObject);
        isProcessing = false;  // 処理が完了したのでフラグをリセット
    }

    private void UpdateOriginalObject(GameObject original, GameObject duplicate)
    {
        // オブジェクトの更新
        if (original.CompareTag("Movable"))
         {
            Debug.Log($"find movable wall, {original.name}");
            photonView = GetComponent<PhotonView>();
            if (photonView != null){
                if (!duplicate.activeSelf)
                {
                    Debug.Log($"{duplicate.name} is not active, try to remove {original.name}");
                    if (photonView.IsMine)
                    {
                        photonView.RPC("RemoveRealWallRPC", RpcTarget.All, original);
                    }
                }
            }
        }

        // 子オブジェクトの更新
        for (int i = 0; i < Mathf.Min(original.transform.childCount, duplicate.transform.childCount); i++)
        {
            Transform originalChild = original.transform.GetChild(i);
            Transform duplicateChild = duplicate.transform.GetChild(i);
            if (originalChild.gameObject.CompareTag("Movable"))
            {
                Debug.Log($"find movable wall, {originalChild.gameObject.name}");
                photonView = GetComponent<PhotonView>();
                if (photonView != null){
                    if (!duplicateChild.gameObject.activeSelf)
                    {
                        Debug.Log($"{duplicateChild.gameObject.name} is not active, try to remove {originalChild.gameObject.name}");
                        if (photonView.IsMine)
                        {
                            photonView.RPC("RemoveRealWallRPC", RpcTarget.All, originalChild.gameObject);
                        }
                    }
                }
            }
            
            //孫以降には再帰的にやる
            if (originalChild.childCount > 0)
            {
                UpdateOriginalObject(originalChild.gameObject, duplicateChild.gameObject);
            }
        }
    }

    [PunRPC]
    private void RemoveRealWallRPC(GameObject wall)
    {
        wall.SetActive(false);
        Debug.Log($"deleted wall name is {wall.name}");
    }

    // コルーチン：指定した時間でアバターの位置を更新
    private IEnumerator UpdateAvatarPosition()
    {
        int sliderValue = Mathf.Clamp((int)slider.value * 10, 0, positionData.LengthPositions() - 1);
        startindex = sliderValue;
        float timeElapsed = 0f;

        while (duplicatedAvatar != null && timeElapsed < 10f)
        {
            if (startindex >= positionData.positions.Count)
            {
                startindex = 0;  // インデックスが範囲を超えた場合リセット
            }

            duplicatedAvatar.transform.position = positionData.GetPosition(startindex) + difforigin;  // アバターの位置を更新
            duplicatedAvatar.transform.rotation = rotationData.GetRotation(startindex);  // アバターの回転を更新
            startindex++;
            timeElapsed += 0.1f;

            yield return new WaitForSeconds(0.1f);  // 0.1秒ごとに更新
        }
    }

    // アバターをスムーズに移動させる
    private void SmoothMove()
    {
        Vector2 input = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);  // 右スティックの入力を取得
        Vector3 direction = new Vector3(input.x, 0, input.y);  // 入力を方向ベクトルに変換
        direction = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * direction;  // カメラの向きに合わせて方向を変換
        duplicatedAvatar.transform.position += direction * moveSpeed * Time.deltaTime;  // 入力に基づいてアバターをスムーズに移動
    }
}