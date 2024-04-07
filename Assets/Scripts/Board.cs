using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap {  get; private set; } // The tilemap child of the Board game object

    public Piece activePiece { get; private set; }// the currently active piece in play (Tetromino)
    public Piece nextPiece { get; private set; }// the next piece (Tetromino)
    public Piece savedPiece { get; private set; }// the saved piece (Tetromino) 
    
    private ScoreUI scoreUI;
    public int score = 0; //score varaible

    private TetrominoData activePieceData; // data for the active piece
    private TetrominoData nextPieceData; // data for the next piece
    private TetrominoData savedPieceData; // data for the saved piece

    public TetrominoData[] tetrominoes; // An array that contains the information of all 7 possible tetrominoes. 

    public Vector3Int activeSpawnPosition;// the spawn position for the currently active tetromino.
    public Vector3Int nextSpawnPosition;// the spawn position for the next tetromino.
    public Vector3Int savedSpawnPosition;// the spawn position for the saved tetromino.
    private int randomActive;// random number used for the active piece
    private int randomNext = -1; // random number set for the next piece. -1 signifies null

    public Vector2Int boardSize = new Vector2Int(10, 20);// The size of the game board
     private ScoreUI scoreUI;

    public int score = 0; //score varaible

    public RectInt Bounds// The bounds of the board which pieces can not move beyond.
    { get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        } 
    }



    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();// Retrieves the tilemap component from the Board GameObject
        this.activePiece = GetComponentInChildren<Piece>();// Retrieves the Piece component from the Board GameObject
        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();// Initializes all the tetrominoes in the tetrominoes array
        }
    }

    private void Start()
    {
        SpawnPiece();// spawns a new piece at the start of the game.
<<<<<<< Updated upstream
        scoreUI = FindObjectOfType<ScoreUI>();
=======
         scoreUI = FindObjectOfType<ScoreUI>();
>>>>>>> Stashed changes
    }


    /*
     * A method for spawning pieces on the game board.
     */
    public void SpawnPiece()
    {
        // If randomNext hasn't been assigned (-1), calculate nextPieceData
        if (randomNext == -1)
        {
            randomNext = Random.Range(0, tetrominoes.Length);
            nextPieceData = tetrominoes[randomNext];
        }

        // Assign nextPieceData to activePieceData
        activePieceData = nextPieceData;

        // Choose a new random tetromino for nextPieceData, ensuring it's different from the active piece
        int newRandomNext = Random.Range(0, tetrominoes.Length);
        while (newRandomNext == randomNext)
        {
        newRandomNext = Random.Range(0, tetrominoes.Length);
        }
        randomNext = newRandomNext; //assigns the old randomNext to the new
        nextPieceData = tetrominoes[randomNext];// gets the relative tetromino
        
        if(nextPiece != null) { // if nextPiece isnt null it tries to clear it
            Clear(nextPiece);
            }
        // Initialize nextPiece with nextPieceData and sets it on the board
        nextPiece = GetComponentInChildren<Piece>();
        nextPiece.Initialize(this, nextSpawnPosition, nextPieceData);
        Set(nextPiece);

        // Initialize activePiece with activePieceData and sets it on the board
        activePiece = GetComponentInChildren<Piece>();
        activePiece.Initialize(this, activeSpawnPosition, activePieceData);
        Set(activePiece);
        
    }

    public void SavePiece() // Function to save a piece, called in Piece.cs whenever "R" is pressed
    {
        Vector3Int tempActivePosition;//Temp postion so that the piece stays in the same spot.

        if(activePiece != null){ // if theres an active piece on the board it gets its postion, otherwise it finds it from the variable
            tempActivePosition = activePiece.position;
        }
        else{
            tempActivePosition = activeSpawnPosition;
        }

        if (savedPiece == null)// If saved piece is null then it creates it
        {
            savedPiece = GetComponentInChildren<Piece>();
            // Clone the TetrominoData of the active piece to save it
            savedPieceData = activePieceData.Clone();
            savedPiece.Initialize(this, savedSpawnPosition, savedPieceData);
            Set(savedPiece);

            Clear(activePiece);//clears activePiece
            SpawnPiece(); // recalls SpawnPiece so that the game continues
        }
        else
        {
            // Swap the TetrominoData between activePieceData and savedPieceData
            TetrominoData tempData = activePieceData.Clone(); // Clone active piece data
            activePieceData = savedPieceData.Clone(); // Assign saved piece data to active piece
            savedPieceData = tempData; // Assign original active piece data to saved piece

            // Clears saved piece and then spawns it at the saved postion
            Clear(savedPiece);
            savedPiece.Initialize(this, savedSpawnPosition, savedPieceData);
            Set(savedPiece);
            //clears active piece and then spawns it at the temp active postion
            Clear(activePiece);
            activePiece.Initialize(this, tempActivePosition, activePieceData);
            Set(activePiece);
        }
    }

    /*
     * A method for setting pieces on the board
     * Parameter: The piece to be set.
     */
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)// loop through all the cells in the piece
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;// set the tilePosition using information for piece.cells.
            /*
             * Draw one of the four tiles that make up the tetromino at the tilePosition.
             * the seven tetrominoes are made up of tiles of varying colors each color
             * corresponds to a specific tetromino. Tetrominoes: I = Cyan, O = Yellow, T = Purple, J = Blue, L = Orange, S = Green, Z = Red
             */
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    /*
     * A method for clearing the activePiece from the board.
     * Works exactly the same as setting the piece to the board using the Set() method
     * except that it sets the tile at the tilePosition to null.
     */
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)// loops through the cells that make up the piece
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);// sets the tile at the tilePosition to null.
        }
    }


    /*
     * A method for checking wether a position on the board is valid for movement.
     * Parameters:
     *      -piece. The current activePiece
     *      -position. The position to move to the piece to.
     *      
     * Returns:
     *      - True. If the piece can move to the position.
     *      - False. If the piece cannot move to the position.
     */
    public bool IsValidPosition(Vector3Int position)
    {
        Piece piece = activePiece;
        RectInt bounds = this.Bounds;// the bounds of the game board.

        for(int i = 0; i < piece.cells.Length; i++)// loop through all the cells that make up the tetromino
        {

            Vector3Int tilePosition = piece.cells[i] + position;// the position of the current cell

            if (!bounds.Contains((Vector2Int)tilePosition))// if the tilePosition is out of bounds.
            {
                return false;// the position is invalid
            }

            if (this.tilemap.HasTile(tilePosition))// If another tile is already in the position the current tile is trying to move to.
            {
                return false;// the position is invalid
            }
        }
        return true;// if the above loop completes without issue tha position is valid.

    }public void ClearLines()

    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        while (row < bounds.yMax)
        {
            if(IsLineFull(row)){// if the line is full for the row that we are currently on, then call the line clear function thast down below
                LineClear(row);
                score += 100;
                 if (scoreUI != null)
            {
                scoreUI.UpdateScore(score); // Update score displayed in UI
            }
            } else{ //increase row when its not full
                row++;
            }
        }

    }
    // this function makes it so it checks if the line is full to ret
    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for  (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if (!this.tilemap.HasTile(position)){
                return false;
            }
        }
        return true;
    }
    private void LineClear(int row){
        RectInt bounds = this.Bounds;
        for  (int col = bounds.xMin; col < bounds.xMax; col++)// clears all tge tiles of the line that is being clearedand 
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

       while (row < bounds.yMax){
        for  (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row + 1, 0);// this is grabbing the row above it 
            TileBase above = this.tilemap.GetTile(position);

            position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, above);
        } 
        row++;
       }
    }
     public void ClearLines()

    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        while (row < bounds.yMax)
        {
            if(IsLineFull(row)){// if the line is full for the row that we are currently on, then call the line clear function thast down below
                LineClear(row);
                score += 100;
                 if (scoreUI != null)
            {
                scoreUI.UpdateScore(score); // Update score displayed in UI
            }
            } else{ //increase row when its not full
                row++;
            }
        }

    }
    // this function makes it so it checks if the line is full to ret
    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for  (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if (!this.tilemap.HasTile(position)){
                return false;
            }
        }
        return true;
    }
    private void LineClear(int row){
        RectInt bounds = this.Bounds;
        for  (int col = bounds.xMin; col < bounds.xMax; col++)// clears all tge tiles of the line that is being clearedand 
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

       while (row < bounds.yMax){
        for  (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row + 1, 0);// this is grabbing the row above it 
            TileBase above = this.tilemap.GetTile(position);

            position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, above);
        } 
        row++;
       }
    }
}
