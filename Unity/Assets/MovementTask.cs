using UnityEngine;
 
public class MovementTask : ITutorialTask
{
    public string GetTitle()
    {
        return "基本操作 移動";
    }
 
    public string GetText()
    {
        return "左スティックで移動、右スティックで回転ができます。";
    }
 
    public void OnTaskSetting()
    {
    }
 
    public bool CheckTask()
    {
        Vector2 R_input = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        Vector2 L_input = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
 
        if (0 < (R_input.x * R_input.x + R_input.y * R_input.y) && 0 < (L_input.x * L_input.x + L_input.y * L_input.y)) {
            return true;
        }
 
        return false;
    }
 
    public float GetTransitionTime()
    {
        return 2f;
    }
}