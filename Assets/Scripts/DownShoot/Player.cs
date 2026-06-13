using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }

    public PlayerAim aim { get; private set; } // get; private set; nghĩa là thuộc tính chỉ có thể được gán giá trị trong lớp này, nhưng có thể được truy cập từ bên ngoài lớp.

    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
