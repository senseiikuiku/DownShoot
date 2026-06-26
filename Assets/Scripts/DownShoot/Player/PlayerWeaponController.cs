using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    private const float REFERENCE_BULLET_SPEED = 20f; // Đây là tốc độ mặc định mà từ đó công thích tính mass được tạo ra

    [SerializeField] private Weapon currentWeapon;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        Invoke("EquipStartingWeapon", 1f);
    }


    #region Slots management - Pickup\Drop\Equip

    private void EquipStartingWeapon() => EquipWeapon(0);

    private void EquipWeapon(int index)
    {
        currentWeapon = weaponSlots[index];

        player.weaponVisuals.PlayWeaponEquipAnimation();
    }

    public void PickupWeapon(Weapon weapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            Debug.Log("Not slots available");
            return;
        }

        weaponSlots.Add(weapon);

        player.weaponVisuals.SwitchOnBackupWeaponModel();
    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
            return;

        weaponSlots.Remove(currentWeapon);

        EquipWeapon(0);
    }
    #endregion

    private void Shoot()
    {
        if (!currentWeapon.CanShoot())
            return;

        GameObject newBullet = ObjectPool.instance.GetBullet();

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // Tính mass dựa trên tốc độ mong muốn để đảm bảo lực bắn phù hợp
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;

        StartCoroutine(ReturnBulletAfterDelay(newBullet, 10f));
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    private IEnumerator ReturnBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.instance.ReturnBullet(bullet);
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        // Nếu không thể ngắm chính xác, chỉ cho phép bắn theo phương ngang
        if (player.aim.CanAimPrecisely() == false && player.aim.Target() == null)
            direction.y = 0;

        //weaponHolder.LookAt(aim); // De cho khac
        //gunPoint.LookAt(aim);

        return direction;

    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    public Weapon CurrentWeapon() => currentWeapon;

    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
            {
                return weapon;
            }
        }
        return null;
    }

    public Transform GunPoint() => gunPoint;

    #region Input Events
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += context => Shoot();

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload())
            {
                player.weaponVisuals.PlayReloadAnimation();
            }
        };
    }
    #endregion
}
