internal class Program
{
    private static void Main(string[] args)
    {
        int bSize = 25;
        BoardGen playerBoard = new BoardGen(bSize);
        BoardGen enemyBoard = new BoardGen(bSize);

        Random rand = new Random();
        for(int i = 0; i < bSize; i++)
        {
            while(!enemyBoard.SetupBoard(rand.Next(2,5),rand.Next(bSize),rand.Next(bSize),rand.Next(4))) {}
        }

        enemyBoard.PrintBoard();
        playerBoard.PrintBoard(enemyBoard);

        Console.WriteLine("terminate" + enemyBoard.boardSize);

        while(1 < 0)
        {
            string input = Console.ReadLine();
            if(input == "exit")
                Environment.Exit(0);
            Console.WriteLine();
        }
        Environment.Exit(0);
    }










    private static void LeetCodeCrap() // if this accidentally gets uploaded to github, skill issue on my part.
    {
        
        Random rand = new Random();

        int len = 400; //change this. make sure it is less that or equal to 400
        int oCount = 0;

        for(int k = 0; k < 8; k++)
        {
            Console.Write("[");
            for(int i = 0; i < len; i++)
            {
                int[] nums = new int[len];
                for(int j = 0; j < len; j++)
                {
                    nums[j] = rand.Next(0,len+1) / len;
                }
                Console.Write($"[{String.Join(",",nums)}]");
                if(i == len-1)
                    Console.Write("]");
                else
                    Console.Write(",");
            }
            Console.WriteLine();
            len /= 2;
        }
    }
}