using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private bool isEquipingWeapon;

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

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
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);

        // SwitchOnCurrentWeaponModel();
    }

    private void Update()
    {
        UpdateRigWeight();

        UpdateLeftHandIkWeight();
    }

    public void PlayReloadAnimation()
    {
        if (isEquipingWeapon)
            return;

        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;

        animator.SetFloat("ReloadSpeed", reloadSpeed);
        animator.SetTrigger("Reload");
        ReduceRigWeight();
    }

    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipAnimationType;
        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIK.weight = 0;
        ReduceRigWeight();
        animator.SetFloat("EquipType", ((float)equipType));
        animator.SetTrigger("EquipWeapon");
        animator.SetFloat("EquipSpeed", equipmentSpeed);
        SetBusyEquipingWeaponTo(true);
    }

    public void SetBusyEquipingWeaponTo(bool busy)
    {
        isEquipingWeapon = busy;
        animator.SetBool("BusyEquipingWeapon", isEquipingWeapon);
    }

    public void SwitchOnCurrentWeaponModel()
    {
        int animationLayerIndex = (int)CurrentWeaponModel().holdType;

        SwitchOffWeaponModels();

        SwitchOffBackupWeaponModels();

        if (player.weapon.HasOnlyOneWeapon() == false)
            SwitchOnBackupWeaponModel();

        SwitchAnimationLayer(animationLayerIndex);
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }

    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    public void SwitchOnBackupWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            if (backupWeaponModel.weaponType == weaponType)
            {
                backupWeaponModel.gameObject.SetActive(true);
            }
        }
    }

    private void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            backupWeaponModel.gameObject.SetActive(false);
        }
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
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

    #region Animation Rigging Methods
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

        leftHand_IK_Target.localPosition = targetTransform.localPosition;
        leftHand_IK_Target.localRotation = targetTransform.localRotation;
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

    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;

    public void MaximizeLeftHandIkWeight() => shouldIncrease_LeftHandIKWeight = true;
    #endregion
}