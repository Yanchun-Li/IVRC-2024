using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNotification : MonoBehaviour
{
    public TextMeshProUGUI tmpText;

    // Start is called before the first frame update
    void Start()
    {
        // テキストの内容を設定
        tmpText.text = "あなたはplayer1or2です";

        // テキストの色を設定し、透明度を75%に調整
        Color textColor = tmpText.color;
        // textColor.a = 0.75f; // 透明度を75%に設定
        tmpText.color = textColor;

        // テキストの位置を画面の上部中央に設定
        RectTransform rectTransform = tmpText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1); // アンカーを上部中央に設定
        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = new Vector2(0, -50); // 上端から少し離れるように位置を調整
    }

    void Update()
    {
        
    }
}
