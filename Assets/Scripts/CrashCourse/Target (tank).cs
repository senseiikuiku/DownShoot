using UnityEngine;

public class Target_Tank : MonoBehaviour
{
    [SerializeField] private Material hitMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            gameObject.GetComponent<Renderer>().material = hitMaterial;

            Destroy(collision.gameObject);
        }
    }
}
