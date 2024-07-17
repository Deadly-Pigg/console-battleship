# console-battleship
Making battleship; the board game on windows console. May be used later in other projects.

## GENERAL INFORMATION:
Battleship is a game where you and your opponent place ships on a (usually) 10x10 board, and take turns trying to sink eachothers boats. One person will shout out a cell number and letter (e.g, C4!) and the other will inform them on if they have hit their ship or not. This will continue until one side has successfully sunk all the others ships.
For more information (not that you care) you can visit the [wikipedia page here](https://en.wikipedia.org/wiki/Battleship_(game))

## TODO: 
### Make and setup board
- Make board more legible and user-friendly
- Possible customisable board size and ship count/type?
### Play and complete game
- ?
### Make a bot to automatically play against the player
- Make algorithm decide the next best slot to shoot at (for Hard difficulty)
- Make algorithm strategically place ships down (or make sure they aren't in contact): Low priority
### Misc
- Stop spam from console (or make it look better)
- Optimisations
- Clean up code
- Comments

## COMPLETED TASKS:
### Make and setup board
- User and computer can place ships on their respective boards + (Input validation)
- User can view their board and the enemy board
### Play and complete game
- User can shoot at the computer's board + (Input validation)
- User can achieve a 'Game Over' when all their ships or the enemy ships have been sunk
- Inform user when their ship/the enemy ship has been sunk
- Input validation
### Make a bot to automatically play against the player
- Computer can play the game in a very random manner. (for Easy difficulty)
- Make algorithm track and sink a ship when it hits a cell with a ship on it. (for Normal+ difficulty)
- Expanding upon above: Make it work for stupid players who place ships in-contact with eachother