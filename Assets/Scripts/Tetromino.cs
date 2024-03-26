using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // Define block positions relative to the Tetromino's position
    [SerializeField] private Vector3[] blockPositions;

    public Vector2Int[] GetBlockPositions(Vector3 moveDirection)
    {
        Vector2Int[] movedPositions = new Vector2Int[blockPositions.Length];
        for (int i = 0; i < blockPositions.Length; i++)
        {
            Vector3 movedPosition = blockPositions[i] + transform.position + (Vector3)moveDirection;
            movedPositions[i] = new Vector2Int(Mathf.RoundToInt(movedPosition.x), Mathf.RoundToInt(movedPosition.y));
        }
        return movedPositions;
    }
}
