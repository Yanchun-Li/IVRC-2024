using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//シーン切り替えに使用するライブラリ

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] Text tutorialmessage;
    // Start is called before the first frame update
    void Start()
    {
        tutorialmessage.text = "これからチュートリアルを開始します";   
    }

    // Update is called once per frame
    void Update()
    {
        //Aボタン、Xボタン同時押しでチュートリアルスキップ
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch)){
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        
    }
}
