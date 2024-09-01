using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class playerAtime : MonoBehaviour
{
    public Slider timeSliderA;         // スライダーオブジェクト
    public Text currentTimeLabelA;     // PlayerAの現在時刻を表示するテキストオブジェクト
    private Timer timer;                // Timerクラスのインスタンスを取得
    private SliderTimeController sliderTimeController; // SliderTimeControllerの参照
    private bool timerExist = false;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogWarning("Timerオブジェクトが見つかりません。");
        }
        else{timerExist=true;}
        //Timerオブジェクトを探して取得

        sliderTimeController = FindObjectOfType<SliderTimeController>();
        if(timer != null)
        {        
            // time の 2倍の値を持つスライダー位置を設定
            timeSliderA.value = Mathf.Clamp(timer.realtime * 2, timeSliderA.minValue, timeSliderA.maxValue);
        }

        // テキストの位置と内容を更新
        UpdateCurrentTimeLabel();
    }

    void Update()
    {
        // 必要な場合は、ここで他の処理を行う
        if (timerExist == false)
        {
            timer = FindObjectOfType<Timer>();
            if (timer == null)
            {
                Debug.LogWarning("Timerオブジェクトが見つかりません。");
            }
            else{timerExist=true;
            // time の 2倍の値を持つスライダー位置を設定
            timeSliderA.value = Mathf.Clamp(timer.realtime * 2, timeSliderA.minValue, timeSliderA.maxValue);
            UpdateCurrentTimeLabel();
            }
        }
    }

    void UpdateCurrentTimeLabel()
    {
        
        float time = timer.realtime;

        // スライダーのhandle（〇）の位置を取得
        RectTransform handleRectTransform = timeSliderA.handleRect.GetComponent<RectTransform>();
        Vector3[] worldCorners = new Vector3[4];
        handleRectTransform.GetWorldCorners(worldCorners);
        Vector3 handlePosition = worldCorners[0]; // 左下の座標を取得

        // テキストをスライダー位置の下に配置
        Vector3 textPosition = handlePosition;
        textPosition.y -= 14;  // 20 はスライダーからの距離（必要に応じて調整）

        // テキストオブジェクトの位置を設定
        currentTimeLabelA.transform.position = textPosition;

        // 時間を分と秒に変換して表示
        int minutes = Mathf.FloorToInt(time * 2 / 60); // 分
        int seconds = Mathf.FloorToInt(time * 2 % 60); // 秒

        // テキストに時間を表示
        currentTimeLabelA.text = string.Format("playerA: {0:D2}:{1:D2}", minutes, seconds);
    }
}
