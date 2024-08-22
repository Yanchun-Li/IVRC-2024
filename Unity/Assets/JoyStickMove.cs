using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class JoyStickMove : MonoBehaviour
{
    [SerializeField] private float movespeed = 0.5f;
    void Update()
    {
        SmoothMove();
    }

    void Move()
    {
        //右ジョイスティックの情報取得
        Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        Vector3 changePosition = new Vector3((movespeed*stickR.x), 0, (movespeed*stickR.y));
        //HMDのY軸の角度取得
        Vector3 changeRotation = new Vector3(0, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.y, 0);
        //OVRCameraRigの位置変更
        this.transform.position -= this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
    }
    void SmoothMove()
    {
        Vector2 input = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);   // 右ジョイスティックの情報取得
        Vector3 direction = new Vector3(input.x, 0, input.y);           // インプットから方向ベクトルを取得

        direction = Quaternion.Euler(0, transform.eulerAngles.y, 0) * direction; // HMDのY軸の角度に基づいて角度を修正
        transform.position -= direction * movespeed * Time.deltaTime;   // 位置変更
    }
}