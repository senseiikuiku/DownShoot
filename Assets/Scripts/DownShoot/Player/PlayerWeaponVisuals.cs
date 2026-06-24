using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private bool isGrabbingWeapon;

    [SerializeField] private WeaponModel[] weaponModels;

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
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);

        SwichOn();
        SwitchAnimationLayer(1);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        UpdateRigWeight();

        UpdateLeftHandIkWeight();
    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel currentWeapon = null;

        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
            {
                currentWeapon = weaponModels[i];
            }
        }
        return currentWeapon;
    }

    public void PlayReloadAnimation()
    {
        if (isGrabbingWeapon)
            return;

        animator.SetTrigger("Reload");
        ReduceRigWeight();
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
        animator.SetFloat("WeaponGrabType", ((float)CurrentWeaponModel().grabType));
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

    private void SwichOn()
    {
        SwitchOffWeaponModels();

        CurrentWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    private void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

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
            SwichOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwichOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwichOn();
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwichOn();
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwichOn();
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}