public enum Status
{
    SUCCESS = 0,
    OCCUPIED_CELL = 1,
    OUT_OF_BOUNDS = 2,
}
public enum Direction
{
    INVALID = -1,
    NORTH = 0,
    EAST = 1,
    SOUTH = 2,
    WEST = 3,
}
public enum GameState
{
    HIT = 0,
    MISS = 1,
    ALREADY_SHOT = 2,
    OUT_OF_BOUNDS = 3,
    PLAYER_WIN = 4,
    ENEMY_WIN = 5,
}