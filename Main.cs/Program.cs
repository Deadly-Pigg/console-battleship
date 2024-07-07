using System.Text.RegularExpressions;
using static Ships;
internal class Program
{
    private static void Main(string[] args)
    {
        int bSize = 10; //default board size.

        BoardGen playerBoard = new BoardGen(bSize); //instantiating
        BoardGen enemyBoard = new BoardGen(bSize);
        Random rand = new Random();
        GameState state;

        Ships ships = new Ships(); //global ships
        ships.AddShip(
        [
            new ShipType("Destroyer", 1, 2),
            new ShipType("Submarine", 1, 3),
            new ShipType("Cruiser", 0, 3),
            new ShipType("Battleship", 0, 4),
            new ShipType("Carrier", 0, 5),
        ]);

        while(true)
        {
            int slots = PlaceShips(playerBoard, new Ships(ships)); //let player make board.
            foreach(ShipType ship in ships.AllShips)
            {
                for(int i = 0; i < ship.Count; i++)
                {
                    Status temp = (Status)1;
                    while(temp != 0)
                        temp = enemyBoard.SetupBoard(ship.Length, rand.Next(0, bSize), rand.Next(0, bSize), (Direction)rand.Next(0,3), ship.Name);
                }
            }
            //Console.WriteLine(slots);
            //enemyBoard.PrintBoard(); //debugging reasons; remove if you will actually play the game
            state = PlayGame(playerBoard, enemyBoard, slots);
            if(state == GameState.PLAYER_WIN)
                Console.WriteLine("PLAYER HAS WON!");
            else
                Console.WriteLine("COMPUTER HAS WON!");
            break;
        }

        Console.WriteLine("terminate" + bSize);
        playerBoard.PrintBoard();
        playerBoard.PrintBoard(playerBoard);
        Environment.Exit(0);
    }
    private static GameState PlayGame(BoardGen playerBoard, BoardGen enemyBoard, int shipCount)
    {
        Random rand = new();
        int countP = shipCount;
        int countE = shipCount;
        GameState fired;
        while(countP > 0 && countE > 0)
        {   
            while(countP > 0)
            {
                playerBoard.PrintBoard(enemyBoard);
                //Thread.Sleep(1000);
                Console.WriteLine("Make your shot.");
                string? read = Console.ReadLine();
                if(read.Length != 2 || !char.IsLetter(read[0]) || !char.IsNumber(read[1]))
                {
                    Console.WriteLine("Invalid coordinate. Choose a coordinate provided on the gameboard.");
                    continue;
                }
                fired = playerBoard.FireShot(char.ToUpper(read[0]) - 'A', read[1] - '0', enemyBoard); 
                if((int)fired > 1)
                {
                    if(fired == GameState.ALREADY_SHOT)
                        Console.WriteLine("Cannot shoot at the same location!");
                    else if(fired == GameState.OUT_OF_BOUNDS)
                        Console.WriteLine("Shot is out of bounds.");
                    continue;
                }
                if(fired == GameState.HIT)
                    countE--;
                break;
            }
            //Thread.Sleep(2000);
            while(countE > 0)
            {
                Console.WriteLine("Enemy will now make their shot...");
                //Thread.Sleep(2000);

                fired = (GameState)2; //allow it allow it allow it allow it allow it allow it

                while((int)fired > 1)
                {
                    fired = enemyBoard.FireShot(rand.Next(0, 10), rand.Next(0,10), playerBoard);
                }
                if(fired == GameState.HIT)
                    countP--;
                break;
            }
            //Thread.Sleep(2000);
        }
        if(countE == 0)
            return GameState.PLAYER_WIN;
        return GameState.ENEMY_WIN;
    }

   
    private static int PlaceShips(BoardGen playerBoard, Ships ships)
    {
        Console.WriteLine("Set up board");

        int totalCount = ships.Total;

        while(totalCount > 0)
        {
            string shipName;
            int ship;
            int[] pos;
            Direction dir;
            while(true) //selection of ship type
            {
                Console.WriteLine($"Input a number 1-{ships.GetCount()} for a ship type.");
                for(int i = 0; i < ships.GetCount(); i++)
                    Console.WriteLine($"{i+1}: {ships.AllShips[i].Name},\t {ships.AllShips[i].Count} left. Length: {ships.AllShips[i].Length}");

                string? read = Console.ReadLine();
                if(read?.Length != 1 || read[0] - '0' < 1 || read[0] - '0' > ships.GetCount()) //Input validation; This only works properly for up to 9 unique ships. May use regex in the future to allow an even higher amount of ships to be added.
                {
                    Console.WriteLine("Invalid ship type. Input a number 1-5.");
                    continue;
                }
                ship = read[0] - '0' - 1;
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
                Console.WriteLine($"Input a coordinate and direction on the board of coordinates in the format: (character)(number)(direction), i.e: A8NORTH or E7WEST");

                string? read = Console.ReadLine();
                if(read.Length < 3 || !char.IsLetter(read[0]) || !char.IsNumber(read[1]))
                {
                    Console.WriteLine("Invalid coordinate.");
                    continue;
                }
                switch(read[2..].ToUpper())
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
                read = read.ToUpper();
                pos = [read[0] - 'A', read[1] - '0'];

                Status valid = playerBoard.SetupBoard(ships.AllShips[ship].Length, pos[0], pos[1], dir, shipName);
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
        return ships.GetCount();
    }
}