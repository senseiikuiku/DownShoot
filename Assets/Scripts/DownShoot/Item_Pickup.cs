using UnityEngine;

public class Item_Pickup : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weapon); // Sử dụng null-conditional operator để tránh lỗi nếu PlayerWeaponController không tồn tại
    }
}
