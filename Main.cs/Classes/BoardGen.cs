
//primarily used for setting up the board
public class BoardGen
{
    public int boardSize {get; private set;} //defines the size of the board.
    protected int[][] board; //defines the board.
    public BoardGen(int setSize = 10) //constructor. setSize may be used in the future for custom game sizes.
    {
        this.boardSize = setSize;
        board = new int[boardSize][];
        for(int i = 0; i < boardSize; i++)
            board[i] = new int[boardSize];
    }
    public void PrintBoard() //The 'getter' for the board. Prints the current state of the board.
    {
        char[] states = {' ','.','#'}; //for the states of each cell in the board:
        /*   == Free Slot
        *  . == Shot
        *  # == Hit a ship
        */
        Console.Write("\t" + String.Join(",", Enumerable.Range(0,boardSize).ToArray()) + "\n\n");

        for(int i = 0; i < boardSize; i++) //im not a psychopath; I'm not gonna use pure LINQ to print EVERYTHING
            Console.WriteLine($"{i}:\t" + String.Join(",", board[i].Select(a => states[a]).ToArray()));
    }
    
}