using UnityEngine;
using Oculus.Interaction;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class slidercontroller : MonoBehaviourPunCallbacks
{
    public OVRInput.Controller controller;  // Oculus コントローラー
    public LayerMask uiLayerMask;           // UI Layer のマスク
    private Slider activeSlider;            // 現在アクティブなスライダー
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Aボタンが押されている間
        if (OVRInput.Get(OVRInput.Button.One))
        {
            Ray ray = new Ray(OVRInput.GetLocalControllerPosition(controller), OVRInput.GetLocalControllerRotation(controller) * Vector3.forward);
            RaycastHit hit;

            // UI のみを対象とするレイキャスト
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, uiLayerMask))
            {
                // スライダーにヒットした場合
                if (hit.collider != null && hit.collider.GetComponent<Slider>() != null)
                {
                    activeSlider = hit.collider.GetComponent<Slider>();
                    UpdateSliderValue(hit);
                }
            }
        }
        else
        {
            activeSlider = null;
        }
    }

    void UpdateSliderValue(RaycastHit hit)
    {
        if (activeSlider != null)
        {
            // スライダーのローカル空間でヒットした位置を取得
            Vector3 localHitPoint = activeSlider.GetComponent<RectTransform>().InverseTransformPoint(hit.point);

            // スライダーの幅を基にして、値を計算
            float newValue = Mathf.InverseLerp(-activeSlider.GetComponent<RectTransform>().rect.width / 2, activeSlider.GetComponent<RectTransform>().rect.width / 2, localHitPoint.x);

            // スライダーの値を更新
            activeSlider.value = Mathf.Lerp(activeSlider.minValue, activeSlider.maxValue, newValue);
        }
    }
}