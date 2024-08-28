public class IOManager
{
    public static int InputNumber(int lowerLim, int upperLim, string msg)
    {
        Console.WriteLine(msg);
        string? read;
        while(true)
        {
            read = Console.ReadLine();
            if(read == null)
                continue;
            if(read.ToLower() == "quit")
                return -1;
            if(!int.TryParse(read, out int num))
                Console.WriteLine($"Invalid input, please provide a number in the range: {lowerLim} - {upperLim}");
            else if(num < lowerLim || num > upperLim)
                Console.WriteLine($"Number outwith range: {lowerLim} - {upperLim}");
            else
                return num;
        }
    }
    public static string[] InputText(int[] argLims, string msg)
    {
        Console.WriteLine(msg);
        string? read;
        while(true)
        {
            read = Console.ReadLine();
            if(read == null)
                continue;
                
            if(read.ToLower() == "quit")
                return ["quit"];
            if(read.ToLower() == "finish")
                return ["finish"];

            if(argLims.Length*2-1 > read.Length)
            {
                Console.WriteLine($"Invalid input, input provided was too short.");
                continue;
            }

            string[] split = read.Split(" ");

            if(split.Length != argLims.Length)
            {
                Console.WriteLine($"Invalid input, input provided had too many arguments/not enough arguments. Please provide {argLims.Length} arguments");
                continue;
            }
            int valid = 0;
            for(int i = 0; i < split.Length; i++)
            {
                if(argLims[i] > 0)
                {
                    if (!int.TryParse(split[i], out int num))
                    {
                        Console.WriteLine($"Error at argument {i}, provided input was not a number");
                        break;
                    }
                    else if (num < 0 || num > argLims[i])
                    {
                        Console.WriteLine($"Number outwith range: 0 - {argLims[i]}");
                        break;
                    }
                }
                else if(argLims[i] != 0)
                {
                    if(-argLims[i] < split[i].Length)
                    {
                        Console.WriteLine($"Error at argument {i}, provided input was too long");
                        break;
                    }
                }
                valid++;
            }
            if(valid == split.Length)
                return split;
        }
        
    }
}