using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftController;
    public Transform rightController;
    public Transform headTarget;
    public Transform cameraTransform;

    public Vector3 handOffset = new Vector3(0, 0, 0.1f); // コントローラーと手の位置関係を調整
    public float headRotationWeight = 0.5f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            // 頭の IK
            if (headTarget != null && cameraTransform != null)
            {
                //animator.SetLookAtWeight(1);
                //animator.SetLookAtPosition(headTarget.position);

                animator.SetLookAtPosition(cameraTransform.position);

                // 頭の回転も設定
                // Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
                // Quaternion currentRotation = animator.GetBoneTransform(HumanBodyBones.Head).rotation;
                // Quaternion newRotation = Quaternion.Slerp(currentRotation, targetRotation, headRotationWeight);
                // animator.SetBoneLocalRotation(HumanBodyBones.Head, newRotation);
            }

            if (leftHand && leftController)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftController.position-leftController.TransformDirection(handOffset));
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftController.rotation * Quaternion.Euler(0, 0, 0));
            }

            if (rightHand && rightController)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightController.position-rightController.TransformDirection(handOffset));
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightController.rotation * Quaternion.Euler(0, 0, 0));
            }
        }
    }
}