using System;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20f; // Đây là tốc độ mặc định mà từ đó công thích tính mass được tạo ra

    private Player player;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;


    private void Start()
    {
        player = GetComponent<Player>();

        player.controls.Character.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // Tính mass dựa trên tốc độ mong muốn để đảm bảo lực bắn phù hợp
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
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

    public Transform GunPoint() => gunPoint;
}
