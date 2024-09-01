using UnityEngine;
using UnityEngine.UI;

public class SliderTimeController : MonoBehaviour
{
    public Slider timeSlider;         // スライダーオブジェクト
    public Text currentTimeLabel;     // playerBの現在時刻を表示するテキストオブジェクト
    private Timer timer;                // Timerクラスのインスタンスを取得
    private float maxTime = 300f;     // スライダーの最大時間 (playerBの現在時刻)

    void Start()
    {
        //Timerオブジェクトを探して取得
        timer=GameObject.FindObjectOfType<Timer>();

        // スライダーの最大値と最小値を設定
        timeSlider.minValue = 0;
        timeSlider.maxValue = maxTime;

        // スライダーの値を現在の時間に設定（現在時刻より右に行かないようにClampで制限）
        timeSlider.value = Mathf.Clamp(timer.realtime-10, 0, maxTime);

        // 現在時刻をテキストに表示
        UpdateCurrentTimeLabel();
    }
void Update()
    {
        // 現在時刻より右に行かないようにスライダーの値を制限
        timeSlider.value = Mathf.Min(timeSlider.value, timer.realtime-10);
    }
    void UpdateCurrentTimeLabel()
    {
        // スライダーの〇の位置を取得
    Vector3 handlePosition = timeSlider.handleRect.position;

    // テキストをスライダーの直下に配置
    Vector3 textPosition = handlePosition;
    textPosition.y -= 20;  // 20 はスライダーからの距離（必要に応じて調整）

    // テキストオブジェクトの位置を設定
    currentTimeLabel.transform.position = textPosition;

        // 時間を分と秒に変換して表示
        int minutes = Mathf.FloorToInt(timer.realtime / 60); // 分
        int seconds = Mathf.FloorToInt(timer.realtime % 60); // 秒

        // テキストに時間を表示
        currentTimeLabel.text = string.Format("playerB:"+"{0:D2}:{1:D2}", minutes, seconds);
    }
}
