public class Ships
{   
    private Tuple<int,int> up = new Tuple<int, int>(0,-1);
    private Tuple<int,int> right = new Tuple<int, int>(1,0);
    private Tuple<int,int> down = new Tuple<int, int>(0,1);
    
    private Tuple<int,int> left = new Tuple<int, int>(-1,0);
    
    public enum ShipType : int
    {
        DESTROYER = 2,
        SUBMARINE = 3,
        //CRUISER = 3,
        BATTLESHIP = 4,
        CARRIER = 5,
    };
    

}