
//primarily used for setting up the board
public class BoardGen
{
    public int boardSize {get; private set;} //defines the size of the board.
    protected int[][] board; //defines the board.

    public BoardGen(int setSize = 10) //constructor. setSize may be used in the future for custom game sizes.
    {
        boardSize = setSize;
        board = new int[boardSize][];
        for(int i = 0; i < boardSize; i++)
            board[i] = new int[boardSize];
    }

    public void PrintBoard() //The 'getter' for the board. Prints the current state of the board.
    {
        char[] states = {' ','+','0','#'}; //for the states of each cell in the board:
        char split = ' ';
        /*   == Free Slot
        *  . == Shot
        *  # == Hit a ship
        */
        Console.Write("\t" + String.Join(split, Enumerable.Range(0,boardSize).Select(a => (char)(a+'A')).ToArray()) + "\n\n");

        for(int i = 0; i < boardSize; i++) //im not a psychopath; I'm not gonna use pure LINQ to print EVERYTHING
            Console.WriteLine($"{i}:\t" + String.Join(split, board[i].Select(a => a < 4 ? states[a] : '?').ToArray()));
    }

    public void PrintBoard(BoardGen enemy) //Shows the state of the enemy's board. Only difference is that it doesn's show hidden ships.
    {
        char[] states = {' ','+',' ','#'}; //for the states of each cell in the board:
        char split = ' ';
        /*   == Free Slot
        *  . == Shot
        *  # == Hit a ship
        */
        Console.Write("\t" + String.Join(split, Enumerable.Range(0,boardSize).Select(a => (char)(a+'A')).ToArray()) + "\n\n");

        for(int i = 0; i < boardSize; i++) //im not a psychopath; I'm not gonna use pure LINQ to print EVERYTHING
            Console.WriteLine($"{i}:\t" + String.Join(split, enemy.board[i].Select(a => a < 4 ? states[a] : '?').ToArray()));
    }

    public GameState FireShot(int x, int y, BoardGen target)
    {
        if(x < 0 || y < 0 || x >= boardSize || y >= boardSize)
            return GameState.OUT_OF_BOUNDS;
            
        if(target.board[y][x] % 2 == 1)
            return GameState.ALREADY_SHOT;

        target.board[y][x]++;
        target.PrintBoard(target);
        Thread.Sleep(1000);
        switch(target.board[y][x])
        {
            case 1: //empty slot.
                Console.WriteLine("Miss.");
                break;
            case 3: //enemy ship.
                Console.WriteLine("Hit!");
                break;
            default:
                throw new Exception("Shot has hit an unknown object, or something it was not supposed to.");
        }

        return target.board[y][x] == 1 ? GameState.MISS : GameState.HIT;
    }

    public Status SetupBoard(int shipType, int x, int y, int direction)
    {
        // Name = shipType, length
        // DESTROYER = 1, 2 
        // SUBMARINE = 2, 3
        // BATTLESHIP = 3, 4
        // CARRIER = 4, 5

        // directions:
        // up == N
        // right == E
        // down == S
        // left == W

        int dX = (direction % 2) * (2 - direction);
        int dY = (1 - direction % 2) * -(1 - direction);

        for(int i = 0; i < shipType; i++) //validating if the current position for the ship is valid. Is for the random generation of a board.
        {
            int newX = x+dX*i;
            int newY = y+dY*i;
            if(newX < 0 || newY < 0 || newX >= boardSize || newY >= boardSize)
            {
                //Console.WriteLine("invalid location; ship will overlap with edge of board");
                return Status.OUT_OF_BOUNDS;
            }
            else if(board[newY][newX] != 0)
            {
                //Console.WriteLine("invalid location; ship will overlap with another ship.");
                return Status.OCCUPIED_CELL;
            }
        }

        for(int i = 0; i < shipType; i++)
            board[y + dY*i][x+dX*i] = 2; // 2 represents a ship

        return Status.SUCCESS;
    }    
}