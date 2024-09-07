using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;

public class SliderTimeController : MonoBehaviourPunCallbacks
{
    public Slider timeSlider;
    public Text currentTimeLabel;
    private Timer timer;
    private float maxTime = 300f;
    private bool timerExist = false;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogWarning("Timerオブジェクトが見つかりません。");
        }
        else{timerExist=true;}
        
        timeSlider.minValue = 0;
        timeSlider.maxValue = maxTime;
        timeSlider.value = 0;

        UpdateCurrentTimeLabel();
        
    }

    void Update()
    {
        if (timerExist == false)
        {
            timer = FindObjectOfType<Timer>();
            if (timer == null)
            {
                Debug.LogWarning("Timerオブジェクトが見つかりません。");
            }
            else{timerExist=true;
            UpdateCurrentTimeLabel();}
        }

        if (timerExist && timer.realtime > 0)
        {
            // 現在時刻より右に行かないようにスライダーの値を制限
            timeSlider.value = Mathf.Min(timeSlider.value,timer.realtime - 10);
            //UpdateCurrentTimeLabel();
        }
    }

    void UpdateCurrentTimeLabel()
    {
        if (!timerExist || timer.realtime <= 0)
        {
            currentTimeLabel.text = "Waiting for players...";
            return;
        }

        Vector3 handlePosition = timeSlider.handleRect.position;
        Vector3 textPosition = handlePosition;
        textPosition.y -= 20;
        currentTimeLabel.transform.position = textPosition;

        int minutes = Mathf.FloorToInt(timeSlider.value / 60);
        int seconds = Mathf.FloorToInt(timeSlider.value % 60);
        currentTimeLabel.text = string.Format("playerB:" + "{0:D2}:{1:D2}", minutes, seconds);
    }
}