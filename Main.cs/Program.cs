internal class Program
{
    private static void Main(string[] args)
    {
        int bSize = 10; //default board size.
        BoardGen playerBoard = new BoardGen(bSize);
        /*
        BoardGen enemyBoard = new BoardGen(bSize);

        Random rand = new Random();
        for(int i = 0; i < bSize; i++)
        {
            while(!enemyBoard.SetupBoard(rand.Next(2,5),rand.Next(bSize),rand.Next(bSize),rand.Next(4))) {}
        }

        enemyBoard.PrintBoard();
        playerBoard.PrintBoard(enemyBoard);*/

        PlayGame(playerBoard);

        Console.WriteLine("terminate" + bSize);
        playerBoard.PrintBoard();
        Environment.Exit(0);
    }

    struct ShipType
    {
        public string Name;
        public int Count;
        public int Length;

        public ShipType(string name, int count, int len)
        {
            Name = name;
            Count = count;
            Length = len;
        }
    }
    private static void PlayGame(BoardGen playerBoard)
    {
        Console.WriteLine("Set up board");

        ShipType[] ships = 
        {
            new ShipType("Destroyer", 1, 2),
            new ShipType("Submarine", 1, 3),
            new ShipType("Cruiser", 1, 3),
            new ShipType("Battleship", 1, 4),
            new ShipType("Carrier", 1, 5),
        };
        int totalCount = 0;
        for(int i = 0; i < ships.Length; i++)
            totalCount += ships[i].Count;

        while(totalCount > 0)
        {
            int ship;
            int[] pos;
            Direction dir;
            while(true) //selection of ship type
            {
                Console.WriteLine($"Input a number 1-{ships.Length} for a ship type.");
                for(int i = 0; i < ships.Length; i++)
                    Console.WriteLine($"{i+1}: {ships[i].Name},\t {ships[i].Count} left. Length: {ships[i].Length}");

                string? read = Console.ReadLine();
                if(read?.Length != 1 || read[0] - '0' < 1 || read[0] - '0' > ships.Length) //Input validation; This only works properly for up to 9 unique ships.
                {
                    Console.WriteLine("Invalid ship type. Input a number 1-5.");
                    continue;
                }
                ship = read[0] - '0' - 1;
                if(ships[ship].Count == 0)
                {
                    Console.WriteLine("No more ships of this type. Please select another ship.");
                    continue;
                }
                ships[(read[0] - '0'-1)].Count--;
                totalCount--;
                break;
            }
            playerBoard.PrintBoard();
            while(true)
            {
                Console.WriteLine($"Input a coordinate and direction on the board of coordinates in the format: (character)(number) (direction), i.e: A8NORTH or E7WEST");
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
                pos = new int[] {read[0] - 'A', read[1] - '0'};

                Status valid = playerBoard.SetupBoard(ships[ship].Length, pos[0], pos[1], (int)dir);
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










    private static void LeetCodeCrap() // if this accidentally gets uploaded to github, skill issue on my part.
    {
        Random rand = new Random();

        int len = 100000; //change this. make sure it is less that or equal to 400

        
        for(int k = 0; k < 8; k++)
        {
            Console.Write("[");
            for(int i = 0; i < len; i++)
            {
                if(i == len-1)
                    Console.Write("{0}", rand.Next(-50,50));
                else
                    Console.Write("{0},", rand.Next(-50,50));
            }
            Console.WriteLine("]");
        }

        Environment.Exit(0);
    }
}