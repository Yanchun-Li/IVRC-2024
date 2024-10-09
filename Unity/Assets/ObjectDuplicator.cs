using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Unity.VisualScripting;

public class ObjectDuplicator : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject originalAvatar;
    public GameObject copyAvatar;
    public ObjectPositionData positionData;
    public ObjectRotationData rotationData;
    public Vector3 newPosition;
    public float duration = 10f;
    public Slider slider; //スライダーを参照
    //public float startime=2f; //共有を開始する時刻、ここでは秒単位でOK
    [SerializeField] public List<int> indexlist; //共有を開始するインデックス
    //public List<float> updateindextime; //更新をする時刻（秒）
    public float updateindextime; //更新をする時刻（秒）
    private int accessCount = 0;
    public int startindex = 0;
    public Vector3 difforigin = new Vector3(0.0f, 0.0f, 0.0f); //エラー回避用

    public GameObject duplicatedObject;
    public GameObject duplicatedAvatar;
    private bool isProcessing = false;
    private float moveSpeed = 0.05f;
    public float pasttime = 0.0f;

    private List<float> wallRemovalTimes = new List<float>();
    private Timer playerTimer;

    void Start()
    {
        playerTimer = GameObject.FindObjectOfType<Timer>(); // Timerスクリプトのインスタンスを取得
        newPosition = this.transform.position;
    }

    void Update()
    {
        if (positionData.LengthPositions() > 2)
        {
            difforigin = newPosition - positionData.GetPosition(2); //原点の違い（rotationは考慮しない）
            difforigin.y = 0;
            pasttime += Time.deltaTime; //位置情報を記録し始めてからの時間を記録する
        }

        // 指定されたボタンが押され、かつ現在処理中でない場合に実行
        if (OVRInput.GetDown(OVRInput.Button.One) && !isProcessing)
        {
            updateindextime = pasttime * 2;
            Debug.Log($"update time is {updateindextime}");
            Debug.Log($"real time is {pasttime}");
            //DuplicateAndMove();
        }

        if (duplicatedAvatar != null)
        {
            Debug.Log("duplicated Avatar is null");
            SmoothMove();
        }

        // 壁を消す操作が実行されるタイミングでチェック
        //CheckAndRemoveWall();
    }

    public void DuplicateAndMove()
    {
        if (isProcessing) return; // 既に処理中なら新たに開始しない

        isProcessing = true;
        //duplicatedObject = Instantiate(originalObject, newPosition, originalObject.transform.rotation);
        //duplicatedAvatar = Instantiate(originalAvatar, newPosition, Quaternion.identity);
        //duplicatedAvatar = Instantiate(originalAvatar, newPosition, Quaternion.identity);
        duplicatedAvatar = Instantiate(copyAvatar, newPosition, Quaternion.identity);
        //duplicatedObject.transform.position = newPosition;
        StartCoroutine(UpdateAndDestroy());
        StartCoroutine(UpdateAvatarPosition());
    }

    private IEnumerator UpdateAndDestroy()
    {
        yield return new WaitForSeconds(duration);
        while (pasttime < updateindextime) { }
        UpdateOriginalObject(originalObject, duplicatedObject);
        //Destroy(duplicatedObject);
        Destroy(duplicatedAvatar);
        isProcessing = false; // 処理完了
    }

    private void UpdateOriginalObject(GameObject original, GameObject duplicate)
    {
        // オブジェクトの更新
        if (original.CompareTag("Movable"))
        {
            UpdateTransform(original.transform, duplicate.transform);
            UpdateComponents(original.gameObject, duplicate.gameObject);
        }

        // 子オブジェクトの更新
        for (int i = 0; i < Mathf.Min(original.transform.childCount, duplicate.transform.childCount); i++)
        {
            // Debug.Log($"Number of original.transform.childCount: {original.transform.childCount}");
            // Debug.Log($"Number of duplicate.transform.childCount: {duplicate.transform.childCount}");
            Transform originalChild = original.transform.GetChild(i);
            Transform duplicateChild = duplicate.transform.GetChild(i);

            if (originalChild.CompareTag("Movable"))
            {
                UpdateTransform(originalChild, duplicateChild);
                UpdateComponents(originalChild.gameObject, duplicateChild.gameObject);

                if (!duplicateChild.gameObject.activeSelf)
                {
                    originalChild.gameObject.SetActive(false);
                }
            }

            // 子オブジェクトの位置、回転、スケールを更新
            originalChild.localPosition = duplicateChild.localPosition;
            originalChild.localRotation = duplicateChild.localRotation;
            originalChild.localScale = duplicateChild.localScale;

            // 必要に応じて、他のコンポーネントの状態も更新
            UpdateComponents(originalChild.gameObject, duplicateChild.gameObject);

            // 孫オブジェクトがある場合、再帰的に処理
            if (originalChild.childCount > 0)
            {
                UpdateOriginalObject(originalChild.gameObject, duplicateChild.gameObject);
            }
        }
    }

    private void UpdateTransform(Transform original, Transform duplicate)
    {
        original.localPosition = duplicate.localPosition - difforigin;
        original.localRotation = duplicate.localRotation;
        original.localScale = duplicate.localScale;
    }

    private void UpdateComponents(GameObject original, GameObject duplicate)
    {
        // 例: Rendererコンポーネントの更新
        Renderer originalRenderer = original.GetComponent<Renderer>();
        Renderer duplicateRenderer = duplicate.GetComponent<Renderer>();
        if (originalRenderer != null && duplicateRenderer != null)
        {
            originalRenderer.material = duplicateRenderer.material;
        }

        // 他のコンポーネントの更新もここに追加
        // 例: Rigidbody, Collider, Custom Scriptsなど
    }

    private IEnumerator UpdateAvatarPosition()
    {
        int sliderValue = Mathf.Clamp((int)slider.value * 10, 0, positionData.LengthPositions() - 1);
        startindex = sliderValue;
        float timeElapsed = 0f;
        while (duplicatedAvatar != null && timeElapsed < 10f)
        {
            if (startindex >= positionData.positions.Count)
            {
                startindex = 0;
                Debug.Log("index is reset");
            }
            Debug.Log($"position data{positionData.name}");
            Debug.Log($"startindex:{startindex}");
            Debug.Log($"PositionData.GetPosition:{positionData.GetPosition(startindex)}");
            Debug.Log("difforigin is:" + difforigin);
            duplicatedAvatar.transform.position = positionData.GetPosition(startindex) + difforigin;
            Debug.Log($"duplicatedAvatar.transform.position:{duplicatedAvatar.transform.position}");
            duplicatedAvatar.transform.rotation = rotationData.GetRotation(startindex);
            startindex++;
            timeElapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(duplicatedAvatar); //10秒経過後にアバターを削除
        accessCount++;
        indexlist.Add(startindex);
    }

    public void RecordWallRemovalTime()
    {
        float wallRemovalTime = slider.value * playerTimer.Timerspeed * Time.deltaTime;
        wallRemovalTimes.Add(wallRemovalTime);
        Debug.Log($"Wall removal time recorded: {wallRemovalTime}");
    }

    private void CheckAndRemoveWall()
    {
        foreach (float wallRemovalTime in wallRemovalTimes)
        {
            if (Mathf.Abs(playerTimer.realtime - wallRemovalTime) <= Time.deltaTime)
            {
                if (duplicatedObject != null && duplicatedObject.CompareTag("Movable"))
                {
                    duplicatedObject.SetActive(false);
                    Debug.Log("Wall removed at correct time.");
                }
            }
        }
    }

    void SmoothMove()
    {
        // 获取右手摇杆的输入
        Vector2 input = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        Vector3 direction = new Vector3(input.x, 0, input.y);

        // 将方向矢量与玩家朝向匹配
        direction = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * direction;

        // 按照输入调整duplicatedAvatar的位置
        duplicatedAvatar.transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
