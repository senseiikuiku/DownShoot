using UnityEngine;

public class TankFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - 5);
    }
}
