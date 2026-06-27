using System.Collections;
using UnityEngine;
public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}


[System.Serializable] // Giúp cho lớp Weapon có thể được hiển thị trong Inspector của Unity
public class Weapon
{
    public WeaponType weaponType;

    public int bulletsInMagazine; // Số lượng đạn hiện có trong băng đạn
    public int magazineCapacity; // Sức chứa tối đa của băng đạn
    public int totalReserveAmmo; // Tổng số đạn dự trữ mà người chơi có thể mang theo

    [Range(1, 3)]
    public float reloadSpeed = 1f; // Tốc độ nạp đạn
    [Range(1, 3)]
    public float equipmentSpeed = 1f; // Tốc độ trang bị vũ khí

    [Space]
    public float fireRate = 1f; // Tốc độ bắn
    private float lastShootTime; // Thời gian bắn lần cuối

    public bool CanShoot()
    {
        if (HaveEnoughBullets() && ReadyToShoot())
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }

    private bool ReadyToShoot()
    {
        if (Time.time > lastShootTime + 1f / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    #region Reload Methods 

    public bool CanReload()
    {
        // Kiểm tra xem băng đạn đã đầy chưa
        if (bulletsInMagazine == magazineCapacity)
        {
            return false;
        }
        // Kiểm tra xem còn đạn dự trữ hay không
        if (totalReserveAmmo > 0)
        {
            return true;
        }
        return false;
    }
    public void RefillBullets()
    {
        int bulletsToReload = magazineCapacity;

        // Nếu số đạn cần nạp vượt quá số đạn dự trữ, chỉ nạp số đạn dự trữ còn lại
        if (bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;
        }
    }
    private bool HaveEnoughBullets() => bulletsInMagazine > 0;

    #endregion
}
