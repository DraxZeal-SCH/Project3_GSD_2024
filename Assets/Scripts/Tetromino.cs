using UnityEngine;
using UnityEngine.Tilemaps;


/*
 * An enumerator for containing the different tetrominoes
 */
public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}


/*
 * A Serializable Data structure for containing the data of the individual tetrominoes
 * Fields:
 *      -tetromino. The type of tetromino
 *      -tile. the tile that is used to populate a tetromino's cells. the tiles come in seven colors
 *      -cells. An array of Vector2Int cooridinates that contain the local positions that make up the shape of the tetromino.
 */
[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells {get; private set;} 
    public Vector2Int[,] wallKicks { get; private set;}


    /*
     * A method for initializing a new tetromino.
     * by getting the tetrominoes data from the static class Data which holds local positional data for representing the shape of the 7 tetrominoes.
     */
    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];// gets the positional data for whichever tetromino is being Initialized.
        this.wallKicks = Data.WallKicks[this.tetromino];
    }
    // Constructor for tetrominos. Mostly used for the next piece and saved piece in board class
    public TetrominoData(Tetromino tetromino, Tile tile, Vector2Int[] cells, Vector2Int[,] wallKicks)
    {
        this.tetromino = tetromino;
        this.tile = tile;
        this.cells = cells;
        this.wallKicks = wallKicks;
    }

    // Custom cloning method, mostly used in the board class for next and saved pieces
    public TetrominoData Clone()
    {
        // Perform a deep copy
        TetrominoData clonedData = new TetrominoData(tetromino, tile, (Vector2Int[])cells.Clone(), (Vector2Int[,])wallKicks.Clone());
        return clonedData;
    }
}