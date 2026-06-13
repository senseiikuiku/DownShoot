using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Animator animator;
    private bool isGrabbingWeapon;

    #region Gun Transform region
    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Transform currentGun;

    #endregion

    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;
    private Rig rig;

    [Header("Left Hand IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHand_IK_Target;
    private bool shouldIncrease_LeftHandIKWeight;



    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();

        SwichOn(pistol);
        SwitchAnimationLayer(1);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && isGrabbingWeapon == false)
        {
            animator.SetTrigger("Reload");

            ReduceRigWeight();
        }
        UpdateRigWeight();

        UpdateLeftHandIkWeight();
    }

    private void UpdateLeftHandIkWeight()
    {
        if (shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;
            if (leftHandIK.weight >= 1)
            {
                leftHandIK.weight = 1;
                shouldIncrease_LeftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;
            if (rig.weight >= 1)
            {
                rig.weight = 1;
                shouldIncrease_RigWeight = false;
            }
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = .15f;
    }

    public void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        ReduceRigWeight();
        animator.SetFloat("WeaponGrabType", ((float)grabType));
        animator.SetTrigger("WeaponGrab");

        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        isGrabbingWeapon = busy;
        animator.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }


    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximizeLeftHandIkWeight() => shouldIncrease_LeftHandIKWeight = true;

    private void SwichOn(Transform gun)
    {
        SwitchOffGuns();
        gun.gameObject.SetActive(true);
        currentGun = gun;

        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHand_IK_Target.localPosition = targetTransform.localPosition;
        leftHand_IK_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }


    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwichOn(pistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwichOn(revolver);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwichOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwichOn(shotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwichOn(rifle);
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}

public enum GrabType { SideGrab, BackGrab }
