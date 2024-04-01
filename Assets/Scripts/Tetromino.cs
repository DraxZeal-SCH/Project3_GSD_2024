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
}