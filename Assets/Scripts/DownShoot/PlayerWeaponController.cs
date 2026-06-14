using System;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;


    private void Start()
    {
        player = GetComponent<Player>();

        player.controls.Character.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {


        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        bullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(bullet, 10f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    private Vector3 BulletDirection()
    {
        Vector3 direction = (aim.position - gunPoint.position).normalized;

        // Nếu không thể ngắm chính xác, chỉ cho phép bắn theo phương ngang
        if (player.aim.CanAimPrecisely() == false)
            direction.y = 0;

        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);

        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
    }
}
