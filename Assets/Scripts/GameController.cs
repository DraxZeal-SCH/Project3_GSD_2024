using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameObject[] tetrominoPrefabs; // Prefabs for different Tetrominos

    public Transform spawnPoint; // The point at which new Tetrominos spawn
    public BoardSize boardSizeScript; // Reference to the BoardSize script
    [SerializeField]
    private int tetrominoIndex = 0;

    private GameObject currentTetromino;
    private float fallTimer = 0.0f;
    public float fallSpeed = 1.0f; // Speed at which the Tetrominos fall

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SpawnNextTetromino();
    }

    void Update()
    {
        // Move Tetromino down at regular intervals
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallSpeed)
        {
            MoveTetrominoDown();
            fallTimer = 0.0f;
        }

        // Handle user input for movement
        HandleInput();
    }

    public void SpawnNextTetromino()
    {
        // Instantiate a random Tetromino prefab at the spawn point
        GameObject tetrominoPrefab = tetrominoPrefabs[tetrominoIndex];
        currentTetromino = Instantiate(tetrominoPrefab, spawnPoint.position, Quaternion.identity);
    }

    public bool IsValidPosition(Vector2Int[] blockPositions)
    {
        foreach (Vector2Int blockPos in blockPositions)
        {
            Vector2Int newPos = blockPos + Vector2Int.RoundToInt(currentTetromino.transform.position);

            // Check if the new position is within the boardSize bounds
            if (newPos.x < 0 || newPos.x >= boardSizeScript.size.x || newPos.y < 0 || newPos.y >= boardSizeScript.size.y)
            {
                Debug.Log("Invalid position detected!");
                return false;
            }
        }
        return true;
    }

    public void MoveTetrominoDown()
    {
        if (currentTetromino != null)
        {
            Tetromino tetrominoScript = currentTetromino.GetComponent<Tetromino>();
            if (tetrominoScript != null)
            {
                if (IsValidPosition(tetrominoScript.GetBlockPositions(Vector3.down)))
                {
                    Debug.Log("Gravity");
                    currentTetromino.transform.position += Vector3.down;
                }
                else
                {
                    // Tetromino can't move down further, spawn next Tetromino
                    SpawnNextTetromino();
                }
            }
        }
    }

    public void MoveTetromino(Vector3 moveDirection)
    {
        if (currentTetromino != null)
        {
            Tetromino tetrominoScript = currentTetromino.GetComponent<Tetromino>();
            if (tetrominoScript != null)
            {
                if (IsValidPosition(tetrominoScript.GetBlockPositions(moveDirection)))
                {
                    currentTetromino.transform.position += moveDirection;
                }
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveTetromino(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveTetromino(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveTetromino(Vector3.down);
        }
    }
}
