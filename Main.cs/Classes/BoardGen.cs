
//primarily used for setting up the board
public class BoardGen
{
    public int boardSize {get; private set;} //defines the size of the board.
    protected int[][] board; //defines the board.
    protected List<ShipData> ships = new();
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

    public (int, GameState) FireShot(int x, int y, BoardGen target, bool isPlayer = false)
    {
        if(x < 0 || y < 0 || x >= boardSize || y >= boardSize)
            return (0,GameState.OUT_OF_BOUNDS);
            
        if(target.board[y][x] % 2 == 1)
            return (0,GameState.ALREADY_SHOT);

        GameState shotStatus;
        int shipLen = 0;
        target.board[y][x]++;
        if(isPlayer)
            target.PrintBoard(target); //shows enemy board (unhit ships invisible)
        else
            target.PrintBoard(); //shows board with all ships
        Thread.Sleep(500);
        switch(target.board[y][x])
        {
            case 1: //empty slot.
                Console.WriteLine("Miss.");
                shotStatus = GameState.MISS;
                break;
            case 3: //enemy ship.
                (shipLen, shotStatus) = FindShip(x,y,target);
                break;
            default:
                throw new Exception("Shot has hit an unknown object, or something it was not supposed to.");
        }

        return (shipLen, shotStatus);
    }
    private (int, GameState) FindShip(int x, int y, BoardGen target)
    {
        //Console.WriteLine($"({x},{y})");
        for(int i = 0; i < target.ships.Count; i++)
        {
            //Console.WriteLine(String.Join(",",target.ships[i].ShipCells));
            if(target.ships[i].ShipCells.Contains((x,y)))
            {
                if(target.ships[i].HasSunk(x,y))
                {
                    Console.WriteLine($"{target.ships[i].Name} has been sunk!\nThere are currently only {target.ships.Count-1} ships left on the target's board!");
                    int len = target.ships[i].Length;
                    target.ships.RemoveAt(i);
                    return (len,GameState.SINK);
                }
                Console.WriteLine("Hit!");
                return (0,GameState.HIT);
            }
        }
        throw new Exception("Board registered a hit, but the ships didn't");
    }

    public Status SetupBoard(int len, int x, int y, Direction dir, string shipName)
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
        int direction = (int)dir;
        int dX = direction % 2 * (2 - direction);
        int dY = (1 - direction % 2) * -(1 - direction);

        for(int i = 0; i < len; i++) //validating if the current position for the ship is valid. Is for the random generation of a board.
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


        for(int i = 0; i < len; i++)
            board[y + dY*i][x+dX*i] = 2; // 2 represents a ship

        ships.Add(new ShipData(x,y,len,dir,shipName));

        return Status.SUCCESS;
    }    
}