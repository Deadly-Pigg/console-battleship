//for ship coordinates
public class ShipData
{
    public HashSet<(int,int)> ShipCells = new();
    public string Name;
    public int Length;
    public ShipData(int startX, int startY, int length, Direction dir, string name)
    {
        int direction = (int)dir;
        int dX = direction % 2 * (2 - direction);
        int dY = (1 - direction % 2) * - (1 - direction);

        for(int i = 0; i < length; i++) //validating if the current position for the ship is valid. Is for the random generation of a board.
        {
            int newX = startX+dX*i;
            int newY = startY+dY*i;
            ShipCells.Add((newX,newY));
        }
        Name = name;
        Length = length;
    }
    public bool HasSunk(int x, int y)
    {
        ShipCells.Remove((x,y));
        return ShipCells.Count() == 0;
    }
}