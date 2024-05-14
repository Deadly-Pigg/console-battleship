internal class Program
{
    private static void Main(string[] args)
    {
        BoardGen boardGen = new BoardGen();
        boardGen.PrintBoard();
        Console.WriteLine("terminate" + boardGen.boardSize);
        Environment.Exit(0);
    }
}