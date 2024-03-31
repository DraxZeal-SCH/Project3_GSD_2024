using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap {  get; private set; } // The tilemap child of the Board game object
    public Piece activePiece { get; private set; }// the currently active piece in play (Tetromino)
    public TetrominoData[] tetrominoes; // An array that contains the information of all 7 possible tetrominoes. 
    public Vector3Int spawnPosition;// the spawn position for the currently active tetromino.
    public Vector2Int boardSize = new Vector2Int(10, 20);// The size of the game board

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
    }


    /*
     * A method for spawning pieces on the game board.
     */
    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);//chooses a random integer between 0 and the length of the tetrominoes array.
        TetrominoData data = this.tetrominoes[random];//chooses a random tetrominoe from the array. and assigns its data to the data variable.

        this.activePiece.Initialize(this, this.spawnPosition, data);// initializes the active piece. passing in the board, spawn position, and the data for the piece.
        Set(this.activePiece);// calls the set method to set the piece on the board and draw the tiles that make up the piece.

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
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
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

    }
}
