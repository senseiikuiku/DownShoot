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
    public float ammo;
    public float maxAmmo;
}
