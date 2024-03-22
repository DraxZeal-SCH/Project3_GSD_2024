using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{   
    [SerializeField]
    private float fallSpeed = 1.0f; // Speed at which the Tetromino falls
    private float fallTimer = 0.0f;
    private GameController gameController;

    
    // Define block positions relative to the Tetromino's position
    private Vector2Int[] blockPositions;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 10, 0);
        gameController = GameController.Instance;

        
        // Example block positions (adjust as needed)
        blockPositions = new Vector2Int[]
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1)
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Move the Tetromino down at regular intervals
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallSpeed)
        {
            Fall();
            fallTimer = 0.0f;
        }

        // Handle user input for movement and rotation
        HandleInput();
    }
    void Fall()
    {
        // Move Tetromino down
        transform.position += new Vector3(0, -1, 0); // Adjust the movement increment according to your grid size
        // Check for collision with other Tetrominoes or bottom of the game board
        if (!IsValidPosition(transform.position))
        {
            // If collision detected, move Tetromino back up and spawn a new Tetromino
            transform.position -= new Vector3(0, -1, 0);
            gameController.SpawnNextTetromino(); // You'll need a gameController script to handle spawning new Tetrominoes
            enabled = false; // Disable this Tetromino script
        }
    }
    void HandleInput()
    {
    // Handle user input for left, right, and rotation
    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
    {
        // Move Tetromino left
        if (IsValidPosition(Vector3.left))
            transform.position += Vector3.left; // Move left only if the new position is valid
    }

    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
    {
        // Move Tetromino right
        if (IsValidPosition(Vector3.right))
            transform.position += Vector3.right; // Move right only if the new position is valid
    }

    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
    {
        // Move Tetromino down
        if (IsValidPosition(Vector3.down))
            transform.position += Vector3.down; // Move down only if the new position is valid
    }

    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
    {
        // Rotate Tetromino clockwise
        transform.Rotate(0, 0, -90); // Rotate 90 degrees clockwise
    }
    }
    bool IsValidPosition(Vector3 moveDirection)
    {
        // Check each block position after applying moveDirection
        foreach (Vector2Int blockPos in blockPositions)
        {
            Vector2Int newPos = blockPos + Vector2Int.RoundToInt(transform.position + moveDirection);
            
            if (newPos.x < -5 || newPos.x > 4 || newPos.y < -10 || newPos.y > 9)
            return false;
            
            // Check if the new position is occupied by another Tetromino
            if (gameController.IsOccupied(newPos))
                return false;
        }

        return true; // All blocks are within valid positions
    }
}
