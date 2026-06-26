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
    public float reloadSpeed = 1f;
    [Range(1, 3)]
    public float equipmentSpeed = 1f;

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }
        return false;
    }

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
        {
            return false;
        }

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
}
