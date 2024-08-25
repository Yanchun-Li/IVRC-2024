using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using Photon.Pun;

public class ObjectDuplicator : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject originalAvatar;
    public ObjectPositionData positionData;
    public ObjectRotationData rotationData;
    public Vector3 newPosition;
    public float duration = 10f;
    public float startime=2f; //共有を開始する時刻、ここでは秒単位でOK

    private GameObject duplicatedObject;
    public GameObject duplicatedAvatar;
    private bool isProcessing = false;

    void Update()
    {
        // 指定されたボタンが押され、かつ現在処理中でない場合に実行
        if (OVRInput.GetDown(OVRInput.Button.One) && !isProcessing)
        {
            Debug.Log("push A button");
            DuplicateAndMove();
        }

        if (duplicatedAvatar != null)
        {
            SmoothMove();
        }
    }

    public void DuplicateAndMove()
    {
        if (isProcessing) return; // 既に処理中なら新たに開始しない

        isProcessing = true;
        duplicatedObject = Instantiate(originalObject, originalObject.transform.position, originalObject.transform.rotation);
        // duplicatedAvatar = Instantiate(originalAvatar, newPosition, Quaternion.identity);
        duplicatedAvatar = PhotonNetwork.Instantiate(originalAvatar.name, newPosition, Quaternion.identity);
        duplicatedObject.transform.position = newPosition;

        StartCoroutine(UpdateAndDestroy());
        StartCoroutine(UpdateAvatarPosition());
        
    }

    private IEnumerator UpdateAndDestroy()
    {
        yield return new WaitForSeconds(duration);
        UpdateOriginalObject(originalObject, duplicatedObject);
        Destroy(duplicatedObject);
        Destroy(duplicatedAvatar);
        isProcessing = false; // 処理完了
    }

    private void UpdateOriginalObject(GameObject original, GameObject duplicate)
    {
        // 親オブジェクトの更新
        original.transform.position = duplicate.transform.position;
        original.transform.rotation = duplicate.transform.rotation;
        original.transform.localScale = duplicate.transform.localScale;

        // 子オブジェクトの更新
        for (int i = 0; i < original.transform.childCount; i++)
        {
            Transform originalChild = original.transform.GetChild(i);
            Transform duplicateChild = duplicate.transform.GetChild(i);

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

    private IEnumerator UpdateAvatarPosition(){
        //Avatar2の動きを反映
        float floatindex = startime/0.5f;
        int startindex = (int)floatindex;
        Vector3 difforigin = newPosition - positionData.GetPosition(0);//原点の違い（向きは考慮しない）
        while(duplicatedAvatar!=null){
            if (startindex>= positionData.positions.Count){
                startindex = 0;
                Debug.Log("index is clear");
            }
            duplicatedAvatar.transform.position = positionData.GetPosition(startindex)+difforigin;
            duplicatedAvatar.transform.rotation = rotationData.GetRotation(startindex);
            startindex++;
            yield return new WaitForSeconds(0.5f);
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