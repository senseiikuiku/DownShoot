using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;

    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
    }

    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeight();

        // Nộp đạn
    }

    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabbingWeaponTo(false);
    }

    public void ReturnRig()
    {
        visualController.MaximizeRigWeight();
        visualController.MaximizeLeftHandIkWeight();
    }
}
