using UnityEngine;

public class ActivateUICanvas : MonoBehaviour
{
    public GameObject uiCanvas;  // 表示したいUIのCanvasをアサイン

    void Update()
    {
        // OculusのAボタンを押されたかどうかをチェック
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            uiCanvas.SetActive(true);
        }
    }
}
