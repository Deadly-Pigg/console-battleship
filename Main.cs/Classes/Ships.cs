public class Ships
{   
    public List<ShipType> AllShips = new List<ShipType>();
    public int Total {get; private set;}
    public Ships(Ships? s = null) //make deep copy of ship class.
    {
        if(s != null)
        {
            AllShips.AddRange(s.AllShips);
            Total = s.Total;
        }
    }
    public struct ShipType(string name, int len, int count, int max = -1) //ship details.
    {
        public string Name = name;
        public int Length = len;
        public int Count = count;
        public int Max = max == -1 ? count : max;
        public ShipType ReduceCount() //structs are primarily 'get', so doing Count-- doesn't work. This is my way of by-passing this issue.
        {
            return new ShipType(Name, Length, Count-1, Max);
        }
    }
    public void AddShip(ShipType ship)
    {
        AllShips.Add(ship);
        Total += ship.Count;
    }
    public void AddShip(ShipType[] ships)
    {
        AllShips.AddRange(ships);
        for(int i = 0; i < ships.Length; i++)
            Total += ships[i].Count;
    }
    public int GetCount()
    {
        return AllShips.Count();
    }
}