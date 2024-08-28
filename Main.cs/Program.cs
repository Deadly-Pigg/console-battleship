using static Ships;
internal class Program
{
    private static void Main(string[] args)
    {
        int bSize; //defines the 2D board size. Default: 10x10
        Ships ships; //defines the ships places on said board. Default ships lengths: 2,3,3,4,5

        while(true)
        {
            Thread.Sleep(1000);
            string read = IOManager.InputText([0],"Welcome to Console Battleship! Game made by DeadlyPigg.\nType 'play' to play a regular game, 'custom' to make a custom game or 'quit' to quit")[0];
            
            switch(read.ToLower())
            {
                case "play":
                    (bSize,ships) = DefaultBoard();
                    break;
                case "custom":
                    (bSize, ships) = CustomBoard();
                    if(bSize == 0)
                        continue;
                    break;
                case "quit":
                    return;
                default:
                    Console.WriteLine("invalid input.\nType 'play' to play a regular game, 'custom' to make a custom game or 'quit' to quit");
                    continue;
            }
            break;
        }

        BoardGen playerBoard = new(bSize); //instantiating
        BoardGen enemyBoard = new(bSize);
        Difficulty gameDiff = ChooseDifficulty();
        BotOpponent computerOpp = new(gameDiff);
        Random rand = new();
        GameState state;
        
        int totalCells = 0; 
        for(int i = 0; i < ships.AllShips.Count; i++)
            totalCells += ships.AllShips[i].Length * ships.AllShips[i].Count;

        while(true)
        {
            PlaceShips(playerBoard, new Ships(ships)); //let player make board.
            Console.WriteLine(totalCells + " total ship cells.");
            
            if(gameDiff == Difficulty.HARD)
                computerOpp.Initialise(bSize, ships);

            RandomShipPlacement(enemyBoard, ships);

            enemyBoard.PrintBoard(); //debugging reasons; remove if you will actually play the game
            state = PlayGame(playerBoard, enemyBoard, totalCells, computerOpp);
            if(state == GameState.PLAYER_WIN)
            {
                if(gameDiff == Difficulty.IMPOSSIBLE)
                    throw new Exception("Dirty cheater.\nSkill difference.\n   ~Computer");
                Console.WriteLine("PLAYER HAS WON!");
            }
            else
                Console.WriteLine("COMPUTER HAS WON!");
            Thread.Sleep(1000);
            Console.WriteLine("Play again? Y/N");

            while(true)
            {
                string? read = Console.ReadLine();
                if(read == null)
                    continue;
                
                read = read.ToUpper();

                if(read != "Y" && read != "N")
                    Console.WriteLine("Invalid input. Y/N, Y to play again, N to exit");
                else if(read == "Y")
                    break;
                else
                    Environment.Exit(0);
            }
            playerBoard.ClearBoard();
            enemyBoard.ClearBoard();
        }
    }
    private static (int, Ships) CustomBoard()
    {
        Ships tempS = new();
        Console.WriteLine("Welcome to the custom board maker! If at any point you wish to leave this menu, type 'quit'\n");
        Thread.Sleep(1000);
        Console.WriteLine();

        int tempSize = IOManager.InputNumber(1,26,"First, please enter a custom board size, e.g: 12 for a 12x12 board\nNote: 5-10 is recommended (smaller boards are generally more fun to play on).");
        if (tempSize == -1) //exit the custom board maker
            return (0, tempS);

        int remainingCells = (int)Math.Ceiling(tempSize*tempSize*0.3);
        Console.WriteLine($"You have {remainingCells} ship cells available to utilise for custom ships.\nA bit of advice, don't create many small ships since they will be time consuming to place down on the board.");
        

        while(remainingCells > 0)
        {
            Thread.Sleep(1000);
            string[] read = IOManager.InputText([0, tempSize, remainingCells], "Now. please enter a ship name, length and count of the ship in the format (Ship name) (Length) (Ship count), e.g: 'Frigate 3 2' for 2 ships named 'Frigate' to occupy 3 cells (total 6 cells)\nTo exit this menu, type 'finish'");

            if (read[0].ToLower() == "quit")
                return (0, tempS);
            if(read[0].ToLower() == "finish")
                break;
            
            int.TryParse(read[1], out int shipLen);
            int.TryParse(read[2], out int shipCount);
            if(shipLen * shipCount > remainingCells)
            {
                Console.WriteLine($"Provided ship length and cells occupy more space than allowed.\nYou have {remainingCells} cells available for ships to take up (Last input took {shipCount * shipLen} cells)\n");
                continue;
            }
            remainingCells -= shipCount*shipLen;
            tempS.AddShip(new ShipType(read[0], shipLen, shipCount));

            Console.WriteLine($"Created {shipCount} new ship{(shipCount == 1 ? 's' : "")} {read[0]} with length {shipLen}.\nYou have {remainingCells} cells available for ships to take up (Last input took {shipCount * shipLen} cells)");
            Thread.Sleep(1000);
        }
        return (tempSize, tempS);
    }
    private static (int, Ships) DefaultBoard() //for setting up the default ships and boardsize.
    {
        Ships tempS = new Ships();
        tempS.AddShip(
        [
            new ShipType("Destroyer", 2, 1),
            new ShipType("Submarine", 3, 1),
            new ShipType("Cruiser", 3, 1),
            new ShipType("Battleship", 4, 1),
            new ShipType("Carrier", 5, 1),
        ]);
        return (10, tempS);
    }
    private static Difficulty ChooseDifficulty()
    {
        while(true)
        {
            Console.WriteLine("Choose a difficulty: Easy, Normal, Hard or Impossible (E/N/H/I)");
            string? read = Console.ReadLine();
            if(read == null)
                continue; 

            if(read.Length != 1)
            {
                Console.WriteLine("Invalid input provided. E = Easy, N = Normal, H = Hard, I = Impossible.");
                continue;
            }
            switch(char.ToUpper(read[0]))
            {
                case 'E':
                    return Difficulty.EASY;
                case 'N':
                    return Difficulty.NORMAL;
                case 'H':
                    return Difficulty.HARD;
                case 'I':
                    return Difficulty.IMPOSSIBLE;
                default:
                    Console.WriteLine("Invalid difficulty provided. E = Easy, N = Normal, H = Hard, I = Impossible.");
                    continue;
            }
        }
    }
    private static GameState PlayGame(BoardGen playerBoard, BoardGen enemyBoard, int shipCount, BotOpponent computerOpp)
    {
        int countP = shipCount;
        int countE = shipCount;
        GameState fired;
        while(countP > 0 && countE > 0)
        {   
            BoardGen.PrintBoard(enemyBoard);
            Thread.Sleep(1000);
            Console.WriteLine("Make your shot.");
            while(countP > 0)
            {
                string[] read = IOManager.InputText([0,playerBoard.boardSize-1], "Make your shot.");
                if(read == null)
                    continue; 

                int x = char.ToUpper(read[0][0]) - 'A';
                int.TryParse(read[1], out int y);

                (_, fired) = playerBoard.FireShot(x, y, enemyBoard, true); 
                if((int)fired > 1)
                {
                    if(fired == GameState.ALREADY_SHOT)
                        Console.WriteLine("Cannot shoot at the same location!");
                    else if(fired == GameState.OUT_OF_BOUNDS)
                        Console.WriteLine("Shot is out of bounds.");
                    continue;
                }
                /*
                BotOpponent newB = new BotOpponent(Difficulty.EASY);
                fired = newB.BotMove(enemyBoard);*/
                if(fired != GameState.MISS)
                    countE--;
                break;
            }
            if(countE == 0)
                break;
            Thread.Sleep(1500);
            Console.WriteLine("Enemy will now make their shot...");
            Thread.Sleep(1000);
            if(computerOpp.BotMove(playerBoard) != GameState.MISS)
                countP--;
            Thread.Sleep(2000);
            Console.WriteLine("\n\n\n\n");
        }
        if(countE == 0)
            return GameState.PLAYER_WIN;
        return GameState.ENEMY_WIN;
    }
    private static void RandomShipPlacement(BoardGen targetBoard, Ships ships)
    {
        Random rand = new Random();
        int bSize = targetBoard.boardSize;

        foreach(ShipType ship in ships.AllShips)
        {
            for(int i = 0; i < ship.Count; i++)
            {
                Status temp = (Status)1;
                while(temp != 0)
                    temp = targetBoard.SetupBoard(ship.Length, rand.Next(0,bSize), rand.Next(0, bSize), (Direction)rand.Next(0,4), ship.Name);
            }
        }
    }
   
    private static void PlaceShips(BoardGen playerBoard, Ships ships)
    {
        Console.WriteLine("Set up board");
        int totalCount = ships.Total;

        while(totalCount > 0)
        {
            string shipName;
            Direction dir;
            int ship;
            while(true) //selection of ship type
            {
                Console.WriteLine();
                for(int i = 0; i < ships.GetCount(); i++)
                    Console.WriteLine($"{i+1}: {ships.AllShips[i].Name},\t {ships.AllShips[i].Count} left. Length: {ships.AllShips[i].Length}");
                Console.WriteLine($"Or, to randomly place all remaining ships down, type '-1'");

                ship = IOManager.InputNumber(-1, ships.GetCount(), $"Input a number 1-{ships.GetCount()} for a ship type.") - 1;

                if(ship == -2)
                {   
                    RandomShipPlacement(playerBoard, ships);
                    Console.WriteLine("This is how the randomly generated board looks:");
                    Thread.Sleep(1000);
                    playerBoard.PrintBoard();
                    Thread.Sleep(1000);
                    return;
                }

                if(ships.AllShips[ship].Count == 0)
                {
                    Console.WriteLine("No more ships of this type. Please select another ship.");
                    continue;
                }
                ships.AllShips[ship] = ships.AllShips[ship].ReduceCount();
                shipName = ships.AllShips[ship].Name;
                totalCount--;
                break;
            }
            playerBoard.PrintBoard();
            while(true)
            {
                string[] read = IOManager.InputText([-1,playerBoard.boardSize-1,0], $"Input a coordinate and direction on the board of coordinates in the format: (character{(playerBoard.boardSize > 10 ? "(s)" : "")}) (number) (direction), i.e: A 8 NORTH or E 7 WEST");

                switch(read[2].ToUpper())
                {
                    case "NORTH": dir = Direction.NORTH; break;
                    case "EAST": dir = Direction.EAST; break;
                    case "SOUTH": dir = Direction.SOUTH; break;
                    case "WEST": dir = Direction.WEST; break;
                    default: dir = Direction.INVALID; break;
                }
                if(dir == Direction.INVALID)
                {
                    Console.WriteLine("Invalid direction.");
                    continue;
                }
                int x = read[0].ToUpper()[0]- 'A';
                int.TryParse(read[1], out int y);
                
                Status valid = playerBoard.SetupBoard(ships.AllShips[ship].Length, x, y, dir, shipName);
                switch(valid)
                {
                    case Status.OCCUPIED_CELL: Console.WriteLine("Invalid location; ship will overlap with another ship."); break;
                    case Status.OUT_OF_BOUNDS: Console.WriteLine("Invalid location; ship will be out of bounds"); break;
                    case Status.SUCCESS: Console.WriteLine("Success! Board now looks like this: "); break;
                }
                if(valid == Status.SUCCESS)
                {                    
                    playerBoard.PrintBoard();
                    break;
                }
            }
        }
    }
}