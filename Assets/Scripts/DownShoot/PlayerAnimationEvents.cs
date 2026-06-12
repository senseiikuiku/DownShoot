using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver()
    {
        visualController.ReturnRigWeightToOne();

        // Nộp đạn
    }

    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabbingWeaponTo(false);
    }

    public void ReturnRig()
    {
        visualController.ReturnRigWeightToOne();
        visualController.ReturnLeftHandIKWeightToOne();
    }
}
