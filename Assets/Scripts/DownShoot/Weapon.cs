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

public enum ShootType
{
    Single,
    Auto,
    Burst
}


[System.Serializable] // Giúp cho lớp Weapon có thể được hiển thị trong Inspector của Unity
public class Weapon
{
    public WeaponType weaponType;

    [Header("Shooting Specifics")]
    public ShootType shootType; // Loại bắn của vũ khí
    public int bulletsPerShot;
    public float defaultFireRate;
    public float fireRate = 1f; // Tốc độ bắn
    private float lastShootTime; // Thời gian bắn lần cuối

    [Header("Burst Fire")]
    public bool burstAvailable;
    public bool burstActive;

    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .1f;

    [Header("Magazine Details")]
    public int bulletsInMagazine; // Số lượng đạn hiện có trong băng đạn
    public int magazineCapacity; // Sức chứa tối đa của băng đạn
    public int totalReserveAmmo; // Tổng số đạn dự trữ mà người chơi có thể mang theo

    [Range(1, 3)]
    public float reloadSpeed = 1f; // Tốc độ nạp đạn
    [Range(1, 3)]
    public float equipmentSpeed = 1f; // Tốc độ trang bị vũ khí

    [Header("Spread")]
    public float baseSpread = 1; // Độ giãn cơ bản của đạn
    public float maximumSpread = 3; // Độ giãn tối đa của đạn
    public float currentSpread = 2; // Độ giãn hiện tại của đạn

    public float spreadIncreaseRate = .15f; // Tốc độ tăng độ giãn khi bắn liên tục

    private float lastSpreadUpdateTime; // Thời gian cập nhật độ giãn lần cuối
    private float spreadCooldown = 1f; // Thời gian hồi phục độ giãn

    #region Spread Methods
    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValye = Random.Range(-currentSpread, currentSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizedValye, randomizedValye, randomizedValye);
        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        currentSpread =
            Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    #endregion

    #region Burst Methods

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }
        return burstActive;
    }

    public void ToggleBurst()
    {
        if (burstAvailable == false)
            return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }

    #endregion

    public bool CanShoot() => HaveEnoughBullets() && ReadyToShoot();

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
