using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimPrecisely;
    [SerializeField] private bool isLockingTarget;

    [Header("Camera Control")]
    [SerializeField] private Transform cameraTarget;
    [Range(.5f, 1.5f)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1.5f, 3f)]
    [SerializeField] private float maxCameraDistance = 3f;
    [Range(3f, 5f)]
    [SerializeField] private float cameraSensitivity = 5f;

    [Space]

    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 aimInput;
    private RaycastHit lastKnowMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            isAimPrecisely = !isAimPrecisely;

        if (Input.GetKeyDown(KeyCode.L))
        {
            isLockingTarget = !isLockingTarget;
        }

        UpdateAimPosition();
        UpdateCameraPosition();
    }
    public Transform Target()
    {
        Transform target = null;

        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
            target = GetMouseHitInfo().transform;

        return target;
    }

    private void UpdateAimPosition()
    {
        Transform target = Target();
        if (target != null && isLockingTarget)
        {
            aim.position = target.position;
            return;
        }

        aim.position = GetMouseHitInfo().point;
        if (!isAimPrecisely)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position =
            Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }

    public bool CanAimPrecisely()
    {
        if (isAimPrecisely)
            return true;
        return false;
    }



    private Vector3 DesiredCameraPosition()
    {
        float actualCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 cameraDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualCameraDistance);

        desiredCameraPosition = transform.position + cameraDirection * clampedDistance; // Tính toán vị trí mong muốn của camera dựa trên hướng và khoảng cách đã được giới hạn
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnowMouseHit;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
