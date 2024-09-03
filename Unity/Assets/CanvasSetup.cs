using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasSetup : MonoBehaviour
{
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        
        // CenterEyeAnchorを名前で検索
        GameObject centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        
        if (centerEyeAnchor != null)
        {
            Camera vrCamera = centerEyeAnchor.GetComponent<Camera>();
            if (vrCamera != null)
            {
                canvas.worldCamera = vrCamera;
                Debug.Log("Canvas Render Camera set to CenterEyeAnchor");
            }
            else
            {
                Debug.LogError("CenterEyeAnchor does not have a Camera component");
            }
        }
        else
        {
            Debug.LogError("CenterEyeAnchor not found in the scene");
        }
    }
}