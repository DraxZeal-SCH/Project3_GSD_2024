using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; } // The game board wich holds the current state of the board and gives access to methods in the Board class.
    public Vector3Int position {  get; private set; }// The current position of the current piece in play
    public TetrominoData data { get; private set; }// The TetrominoData structure. which holds data about the tetromino that is currently in play
    public int rotationIndex { get; private set; }// The current rotation index for the active piece.
    private bool canMove;

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
        this.rotationIndex = 0;

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
    
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }


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
        //If r is pressed then it calls the SavePiece function from the board class
        if (Input.GetKeyDown(KeyCode.R)){
            this.board.SavePiece();
        }
    
            this.board.Set(this);// Setting the current position of the piece on the board board at the end of every frame.
    }

    /*
     * A method which is called whenever a piece needs to be moved.
     * Parameter: translation. A Vector2Int position holding the translation direction for the movement of the piece.
     * 
     * Return: boolean. Returns whether the movement was carried out successfully or not. True if movement was successful : False if movement failed
     *            
     */
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosision = this.position;// Setting the new position to the current position of the piece.
        newPosision.x += translation.x;// Adding the translation.x position to the newPosition.x (moving the piece to the right or left) 
        newPosision.y += translation.y;// Adding the translation.y position to the newPosition.y (moving the piece up or down. in this case only down)

        bool valid = this.board.IsValidPosition(newPosision);// call to the IsValidPosition method in the Board class

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

    /*
     * Method for applying rotation to the active piece
     * uses a rotation matrix to apply a rotation in a given direction.
     * also tests wether or not to use a wall kick which should stop pieces
     * from moving out of bounds during rotations against the walls of the board
     * Parameter: direction. the direction the piece should rotate given as
     * a value of 1 for rotating clockwise or -1 for counter-clockwise
     */
    private void Rotate(int direction)
    {
        int originalRotationIndex = this.rotationIndex;// Assigning the current rotation index value to the originalRotationIndex variable
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);// Calculating the new rotation index and assigning the new index to the rotationIndex variable

        ApplyRotationMatrix(direction);// Calling the ApplyRotationMatrix method to apply the new rotation to the active piece.

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotationIndex;
            ApplyRotationMatrix(-direction);
        }
    }

    /*
     * A method for applying the rotation matrix found in the Static Data class
     * applies the rotation differently for the I and O tetrominoes.
     */
    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);

        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];
            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection) 
    {
        int wallKickIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if(input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
