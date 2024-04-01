using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; } // The game board wich holds the current state of the board and gives access to methods in the Board class.
    public Vector3Int position {  get; private set; }// The current position of the current piece in play
    public TetrominoData data { get; private set; }// The TetrominoData structure. which holds data about the tetromino that is currently in play

    /* An array of the cells that make up the current tetromino. Each tetromino is made up of four cells.
     * Each cell contains some positional information to inform the game of the shape of the tetromino.
     */
    public Vector3Int[] cells { get; private set; }

    /*
     * A method for Initializing a Piece. A piece in this context is the current tetromino that is in play
     * Parameters:
     *  - board. The current Game board
     *  - position. The Starting position for a new Piece.
     *  - data. The data needed for a new tetromino piece such as the local positions that make up the shape of the tetromino,
     *          and the tile to be used when making the tetromino.
     */
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position; 
        this.data = data;

        /* A conditional for ensuring that the cells array is initiallized
         * to an empty array with the same number of cells found in a tetromino.
         * Generally tetrominoes are made up of 4 segments. This conditional allows
         * for larger custom shapes if desired.
         */
        if (this.cells == null) // checks if the cells array is equivalent to null.
        {
            this.cells = new Vector3Int[data.cells.Length];// Assigns an empty array of the appropriate length to hold the cells of the new tetromino. 
        }

        for (int i = 0; i < data.cells.Length; i++)// loops through the full Length of data.cells.Length(The number of cells that make up one of the tetrominoes.)
        {
            this.cells[i] = (Vector3Int)data.cells[i];// Assigns the position of each cell in the generated tetromino(data.cells) to the cells array of the current piece.
        }
    }


    private void Update()
    {
        this.board.Clear(this);// Clearing the current position of the piece on the board board at the start of every frame.

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Drop();
        }

            this.board.Set(this);// Setting the current position of the piece on the board board at the end of every frame.
    }

    /*
     * A method which is called whenever a piece needs to be moved.
     * Parameter: translation. A Vector2Int position holding the translation direction for the movement of the piece.
     * 
     * Return: boolean. Returns wether the movement was carried out successfully or not. True if movement was successful : False if movement failed
     *            
     */
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosision = this.position;// Setting the new position to the current position of the piece.
        newPosision.x += translation.x;// Adding the translation.x position to the newPosition.x (moving the piece to the right or left) 
        newPosision.y += translation.y;// Adding the translation.y position to the newPosition.y (moving the piece up or down. in this case only down)

        bool valid = this.board.IsValidPosition(this, newPosision);// call to the IsValidPosition method in the Board class

        if (valid)// if the new position is valid for movement.
        {
            this.position = newPosision;// Set the pieces position to the new position.
        }

        return valid;// return wether the movement succeeded or failed
    }

    /*
     * A method which immediately drops the current piece to the bottom of the board.
     */
    private void Drop()
    {
        while (Move(Vector2Int.down))
        {
            continue;// continues until the piece reaches the bottom of the board.
        }
    }
}
