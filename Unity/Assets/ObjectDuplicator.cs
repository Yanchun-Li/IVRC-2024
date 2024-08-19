using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class ObjectDuplicator : MonoBehaviour
{
    public GameObject originalObject;
    public Vector3 newPosition;
    public float duration = 10f;


    private GameObject duplicatedObject;
    private bool isProcessing = false;

    void Update()
    {
        // 指定されたボタンが押され、かつ現在処理中でない場合に実行
        if (OVRInput.GetDown(OVRInput.Button.One) && !isProcessing)
        {
            Debug.Log("push A button");
            DuplicateAndMove();
        }
    }

    public void DuplicateAndMove()
    {
        if (isProcessing) return; // 既に処理中なら新たに開始しない

        isProcessing = true;
        duplicatedObject = Instantiate(originalObject, originalObject.transform.position, originalObject.transform.rotation);
        duplicatedObject.transform.position = newPosition;
        StartCoroutine(UpdateAndDestroy());
    }

    private IEnumerator UpdateAndDestroy()
    {
        yield return new WaitForSeconds(duration);
        UpdateOriginalObject(originalObject, duplicatedObject);
        Destroy(duplicatedObject);
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
}