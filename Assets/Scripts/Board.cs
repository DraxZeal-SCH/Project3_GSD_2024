using UnityEngine;

public class BoardSize : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(10, 20); // Default board size

    // Visualize the board size using Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 1f));
    }
}
