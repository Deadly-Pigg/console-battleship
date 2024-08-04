
public class BotOpponent(Difficulty diff)
{
    Difficulty Diff = diff; //difficulty of the bot
    Random rand = new Random();
    public GameState BotMove(BoardGen oppBoard) //selects appropriate algorithm based on the user's choice
    {
        switch(Diff)
        {
            case Difficulty.EASY:
                return Easy(oppBoard);
            case Difficulty.NORMAL:
                return Normal(oppBoard);
            case Difficulty.HARD:
                return Hard(oppBoard);
            case Difficulty.IMPOSSIBLE:
                return Impossible(oppBoard);
            default:
                throw new Exception("Invalid difficulty for bot opponent (BotOpponent.cs/BotMove)");
        }
    }

    
    private GameState Easy(BoardGen oppBoard)
    {
        GameState fired = (GameState)2; //allow it allow it allow it allow it allow it allow it

        while((int)fired > 1) //randomly fires until it finds a valid location lmfao
            (_, fired) = oppBoard.FireShot(rand.Next(0, oppBoard.boardSize), rand.Next(0,oppBoard.boardSize), oppBoard);
        return fired;
    }

    HashSet<(int,int)> hitCoords = new();
    bool vertical = false;
    private GameState Normal(BoardGen oppBoard)
    {
        GameState fired = (GameState)2; 
        if(hitCoords.Count > 0)
        {
            fired = Tracking(oppBoard);
        }

        int x = 0, y = 0;
        while((int)fired > 1)
        {
            x = rand.Next(0, oppBoard.boardSize);
            y = rand.Next(0, oppBoard.boardSize);
            (_, fired) = oppBoard.FireShot(x, y, oppBoard);
            if(fired == GameState.HIT)
            {
                hitCoords.Add((x,y));
            }
        }
        
        
        //Console.WriteLine(String.Join(",",hitCoords.ToList()));

        return fired;
    }
    private GameState Tracking(BoardGen oppBoard) //used in medium/hard difficulties.
    {
        GameState fired = (GameState)2;
        int x = 0, y = 0;
        int shoot = rand.Next(0,2);
        int len = 0;
        if(hitCoords.Count() > 1) //tracks hit ships
        {
            List<(int,int)> nextStrike = hitCoords.ToList();
            int min = 999999, max = -1; //defines min/max coords on a line
            (x,y) = nextStrike[0];
            for(int i = 0; i < nextStrike.Count; i++)
            {
                (int tempX, int tempY) = nextStrike[i];
                int prioritise = tempX;
                if(vertical)
                    prioritise = tempY;
                max = Math.Max(max, prioritise);
                min = Math.Min(min, prioritise);
            }

            x = vertical ? x : max+1;
            y = vertical ? max+1 : y;

            if(shoot == 0) //shoot higher than max
                (len, fired) = oppBoard.FireShot(x,y,oppBoard);

            if(shoot == 1 || (int)fired > 1) //shoot lower than min
            {
                shoot = 1;
                x = vertical ? x : min-1;
                y = vertical ? min-1 : y;
                (len, fired) = oppBoard.FireShot(x,y,oppBoard);
            }
            
            if((int)fired > 1) //no target can be hit; retry
            {
                shoot = 0;
                x = vertical ? x : max+1;
                y = vertical ? max+1 : y;
                (len, fired) = oppBoard.FireShot(x,y,oppBoard);
            }
        }
        if((int)fired > 1) //search around previous hit location for a new cell to hit.
        {
            List<(int,int)> nextStrike = new List<(int,int)>{(0,1),(0,-1),(1,0),(-1,0)}; 
            while((int)fired > 1 && nextStrike.Count > 0)
            {
                (x,y) = hitCoords.First();
                int ind = rand.Next(0, nextStrike.Count());
                (int tempX, int tempY) = nextStrike[ind];
                nextStrike.RemoveAt(ind);
                x += tempX;
                y += tempY;
                (len, fired) = oppBoard.FireShot(x, y, oppBoard);
                if(fired == GameState.HIT)
                    vertical = tempX == 0;
            }
        }
        if(fired == GameState.HIT)
        {
            hitCoords.Add((x,y));
        }
        if(fired == GameState.SINK) //removes the sunk ship from the list of ships or whatever
        {
            for(int i = 0; i < len; i++)
            {
                if(shoot == 0) //remove cells going left or up from hitCoords
                    hitCoords.Remove((vertical ? x : x - i, vertical ? y - i : y));
                else
                    hitCoords.Remove((vertical ? x : x + i, vertical ? y + i : y));
            }
        }
        return fired;
    }
    /*
        HARD DIFFICULTY
    */
    int[] currShips;
    int minDist = 0;
    private GameState Hard(BoardGen oppBoard)
    {
        GameState fired = (GameState)2; 
        if(hitCoords.Count > 0)
            fired = Tracking(oppBoard);

        int x, y, boardVal;
        while((int)fired > 1)
        {
            boardVal = rand.Next(0, oppBoard.boardSize * oppBoard.boardSize / minDist) * minDist;
            x = boardVal / oppBoard.boardSize;
            y = boardVal % oppBoard.boardSize;
            (_, fired) = oppBoard.FireShot(x, y, oppBoard);
            if(fired == GameState.HIT)
            {
                hitCoords.Add((x,y));
            }
        }

        if(fired == GameState.SINK)
        {
            currShips[minDist]--;
            while(minDist < currShips.Length && currShips[minDist] == 0)
                minDist++;
            Console.WriteLine(minDist);
        }

        return fired;
    }
    public void Initialise(int boardSize, Ships ships) //for initialising hard difficulties' smart targetting algorithm
    {
        currShips = new int[boardSize];

        foreach(Ships.ShipType ship in ships.AllShips)
            currShips[ship.Length-1] += ship.Count;

        for(int i = 1; i < boardSize; i++)
        {
            if(currShips[i] == 0)
                continue;
            minDist = i;
            return;
        }
    }

    /*
        Honourary 'Fuck you' Difficulty. Code is copied from Easy difficulty
    */
    private GameState Impossible(BoardGen oppBoard)
    {
        GameState fired = GameState.ENEMY_WIN; //Since the bot never loses

        while((int)fired > 0) //if the bot misses, it shoots again
            (_, fired) = oppBoard.FireShot(rand.Next(0, oppBoard.boardSize), rand.Next(0,oppBoard.boardSize), oppBoard);

        return fired;
    }
}