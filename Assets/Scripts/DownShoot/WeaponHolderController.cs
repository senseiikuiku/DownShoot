using UnityEngine;

public class WeaponHolderController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Transform currentGun;

    [Header("Left Hand IK")]
    [SerializeField] private Transform leftHand;

    private void Start()
    {
        SwichOn(pistol);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwichOn(pistol);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwichOn(revolver);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwichOn(autoRifle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwichOn(shotgun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwichOn(rifle);
        }
    }

    private void SwichOn(Transform gun)
    {
        SwitchOffGuns();
        gun.gameObject.SetActive(true);
        currentGun = gun;

        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHand.localPosition = targetTransform.localPosition;
        leftHand.localRotation = targetTransform.localRotation;
    }
}
