using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Game Board Size")]
    [Range(1, 20)] // Adjust the range as needed // Height of the game board
    public int width = 1; // Width of the game board

    [Range(1, 30)] // Adjust the range as needed
    public int height = 1; // Height of the game board

    private int[,] grid; // 2D array representing the game board

    private void Start(){

    }

    private void Update(){

    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeGrid();
    }

    void InitializeGrid()
    {
        // Initialize the game board grid
        grid = new int[width, height];
    }

    public bool IsOccupied(Vector2Int position)
    {
        // Check if the specified position on the game board is occupied by a Tetromino block
        return grid[position.x, position.y] != 0;
    }

    public void SpawnNextTetromino()
    {
        // Spawn the next Tetromino at the top center of the game board
        // You'll need to implement your spawning logic here
        // Example: TetrominoSpawner.Instance.SpawnTetromino();
    }

    public void CheckForCompletedLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineComplete(y))
            {
                // Clear the completed line
                ClearLine(y);
                // Shift down all blocks above the completed line
                ShiftDownAbove(y);
            }
        }
    }

    bool IsLineComplete(int y)
    {
        // Check if the specified line is complete (filled with blocks)
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == 0)
                return false; // Line is not complete
        }
        return true; // Line is complete
    }

    void ClearLine(int y)
    {
        // Clear the specified line by setting all blocks to 0
        for (int x = 0; x < width; x++)
        {
            grid[x, y] = 0;
        }
    }

    void ShiftDownAbove(int startY)
    {
        // Shift down all blocks above the specified line
        for (int y = startY + 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y - 1] = grid[x, y];
                // Clear the original position of the block
                grid[x, y] = 0;
            }
        }
    }

    public void PlaceTetrominoOnGrid(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);
            grid[position.x, position.y] = 1; // Or any other non-zero value to indicate occupancy
        }
    }
    public bool IsValidPosition(Vector2Int position)
    {
        // Check if the specified position is within the boundaries of the game board and not occupied by another block
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height && grid[position.x, position.y] == 0;
    }
    
    public void RemoveCompletedLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineComplete(y))
            {
                ClearLine(y);
                ShiftDownAbove(y);
                y--; // Recheck the same line as it has shifted down
            }
        }
    }

    // Add methods for updating score, managing levels, and handling game over conditions as needed
}
