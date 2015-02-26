using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseNibblerII
{
    class Program
    {
        static void Main(string[] args)
        {
            //creates instance of game
            CheeseNibbler game = new CheeseNibbler();
            game.PlayGame();
            //keeps console open
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Represents the position of an object on the grid4
    /// </summary>
    public class Point
    {
        public enum PointStatus
        {
            Empty, Cheese, Mouse, Cat, CatAndCheese
        }
        
        //Declaring point properties
        public int X { get; set; }
        public int Y { get; set; }
        public PointStatus Status { get; set; }

        //Point constructor
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Status = PointStatus.Empty;
        }

    }

    /// <summary>
    /// Class containing Mouse and its properties
    /// </summary>
    public class Mouse
    {
        //Mouse Properties
        public Point Position { get; set; }
        public int Energy { get; set; }
        public bool HasBeenPouncedOn { get; set; }

        //Mouse constructor
        public Mouse()
        {
            //starting energy
            this.Energy = 50;
            //end game condition
            this.HasBeenPouncedOn = false;
        }
    }

    /// <summary>
    /// Class containing Cat and its properties
    /// </summary>
    public class Cat
    {
        //Point property
        public Point Position { get; set; }

        //Cat constructor with no additional conditions
        public Cat()
        {
        
        }
    }

    /// <summary>
    /// Main game logic
    /// </summary>
    public class CheeseNibbler
    {
        //All necessary properties to run game
        public Random rng { get; set; }//random number generator
        public Point[,] Grid { get; set; }//Grid itself
        public Mouse Mouse { get; set; }//Our Mouse
        public Point Cheese { get; set; }//Sought after cheese
        public int CheeseCount { get; set; }//Pieces of cheese nibbled  
        public List<Cat> Cats { get; set; }//List of Cats to chase Mouse

        /// <summary>
        /// Main game constructor
        /// </summary>
        public CheeseNibbler()
        {
            //initializes mouse
            this.Mouse = new Mouse();
            //initializes random number
            this.rng = new Random(); 

            //initialize the grid
            this.Grid = new Point[10, 10];

            //loop through every x, y value in grid
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    //setting a new Point, into each
                    //coordinate of the grid
                    this.Grid[x, y] = new Point(x, y);
                }
            }

            //Initializes list of Cats  
            this.Cats = new List<Cat>();

            //Places the cheese
            PlaceCheese();
           
            //Place a mouse
            do
            {
                this.Mouse.Position = this.Grid[rng.Next(this.Grid.GetLength(0)), rng.Next(this.Grid.GetLength(1))];
            } while (this.Mouse.Position.Status != Point.PointStatus.Empty);
            this.Mouse.Position.Status = Point.PointStatus.Mouse;
        }

        /// <summary>
        /// Creates grid to play game
        /// </summary>
        public void DrawGrid()
        {
            //Clear the console
            Console.Clear();

            //Populates grid with various point types
            for(int y = 0; y < this.Grid.GetLength(1); y++)
            {
                for(int x = 0; x < this.Grid.GetLength(0); x++)
                {
                    //check and prints type of each point
                    switch (Grid[x, y].Status)
                    {
                        case Point.PointStatus.Mouse:

                            Console.Write("[M]");
                            break;
                        case Point.PointStatus.Cheese:
                            Console.Write("[C]");
                            break;
                        case Point.PointStatus.Cat:
                            Console.Write("[X]");
                            break;
                        case Point.PointStatus.CatAndCheese:
                            Console.Write("[X]");
                            break;
                        default:
                            Console.Write("[ ]");
                            break;
                    }
                }
                //Adds line for each row
                Console.WriteLine();
            }
            //Prints game play messages
            Console.WriteLine("Use the numpad to move the mouse!");
            Console.WriteLine("\nPieces of Cheese: {0}", CheeseCount);
            Console.WriteLine("Mouse Energy: {0}", Mouse.Energy);
        }

        /// <summary>
        /// Takes in player move
        /// </summary>
        /// <returns>The player input</returns>
        public ConsoleKey GetUserMove()
        {
            //gets players key stroke
            ConsoleKey playerInput = Console.ReadKey(true).Key;

            //Condition     
            bool validInput = false;

            //loops through possible keystrokes
            while (!validInput)
            {
                //checks for valid input
                switch (playerInput)
                {
                    case ConsoleKey.NumPad4://left
                    case ConsoleKey.NumPad6://right
                    case ConsoleKey.NumPad8://up
                    case ConsoleKey.NumPad2://down
                    case ConsoleKey.NumPad7://upper left
                    case ConsoleKey.NumPad9://upper right
                    case ConsoleKey.NumPad1://lower left
                    case ConsoleKey.NumPad3://lower right
                        validInput = true;
                        return playerInput;
                    default:
                        //Prints error messsage
                        Console.WriteLine("\nNot a valid move");
                        //reminds player which keys to press
                        Console.WriteLine("Use the numpad to move the mouse");
                        playerInput = Console.ReadKey(true).Key;
                        return playerInput;
                }
            }
            //returns the input 
            return playerInput;
        }

        /// <summary>
        /// Checks if the players input was a valid keystroke
        /// </summary>
        /// <param name="playerInput">Keystroke of player</param>
        /// <returns>Whether or not it was valid</returns>
        public bool ValidMove(ConsoleKey playerInput)
        {
            //look at each type of keypress
            switch (playerInput)
            {
                //checks if the move is on the grid
                case ConsoleKey.NumPad4:
                    return this.Mouse.Position.X > 0;//left
                case ConsoleKey.NumPad6:
                    return this.Mouse.Position.X < 9;//right
                case ConsoleKey.NumPad2:
                    return this.Mouse.Position.Y< 9;//down
                case ConsoleKey.NumPad8:
                    return this.Mouse.Position.Y > 0;//up
                case ConsoleKey.NumPad7:
                    return this.Mouse.Position.Y > 0 && this.Mouse.Position.X > 0;//upper left
                case ConsoleKey.NumPad9:
                    return this.Mouse.Position.Y > 0 && this.Mouse.Position.X < 9;//upper right
                case ConsoleKey.NumPad1:
                    return this.Mouse.Position.Y < 9 && this.Mouse.Position.X > 0;//down left
                case ConsoleKey.NumPad3:
                    return this.Mouse.Position.Y < 9 && this.Mouse.Position.X < 9;//down right
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Game logic to move the mouse based on player's input
        /// </summary>
        /// <param name="playerInput">Keystroke used to move mouse</param>
        /// <returns></returns>
        public bool MoveMouse(ConsoleKey playerInput)
        {
            //creates new positions for the mouse
            int nextX = this.Mouse.Position.X;
            int nextY = this.Mouse.Position.Y;
            //Checks if move was valid
            if (ValidMove(playerInput))
            {
                //if so, moves the mouse accordingly
                switch (playerInput)
                {
                    case ConsoleKey.NumPad4://left
                        nextX -= 1;
                        break;
                    case ConsoleKey.NumPad6://right
                        nextX += 1;
                        break;
                    case ConsoleKey.NumPad2://down
                        nextY += 1;
                        break;
                    case ConsoleKey.NumPad8://up
                        nextY -= 1;
                        break;
                    case ConsoleKey.NumPad7://upper left
                        nextX -= 1;
                        nextY -= 1;
                        break;
                    case ConsoleKey.NumPad9://upper right
                        nextX += 1;
                        nextY -= 1;
                        break;
                    case ConsoleKey.NumPad1://down left
                        nextX -= 1;
                        nextY += 1;
                        break;
                    case ConsoleKey.NumPad3://down right
                        nextX += 1;
                        nextY += 1;
                        break;
                    default:
                        break;
                }
                //decrements mouse 
                //energy after each move
                Mouse.Energy--;
            }
            //sets mouse's next move on the grid
            Point nextMouseMove = this.Grid[nextX, nextY];

            //checks if space has the cheese
            if (nextMouseMove.Status == Point.PointStatus.Cheese)
            {
                //if so, empty old mouse point
                this.Grid[Mouse.Position.X, Mouse.Position.Y].Status = Point.PointStatus.Empty;
                //updates to new mouse point
                this.Grid[nextX, nextY].Status = Point.PointStatus.Mouse;
                this.Mouse.Position = this.Grid[nextX, nextY];
                return true;
            }
                //if the next move is on the same point as the cat or the cat&cheese
            else if (nextMouseMove.Status == Point.PointStatus.Cat)
            {
                //sets mouse has been pounced on to true
                Mouse.HasBeenPouncedOn = true;
                this.Mouse.Position.Status = Point.PointStatus.Cat;     
                return false;
            }
                //otherwise,
            else
            {
                
                //set mouse to the new position
                this.Grid[nextX, nextY].Status = Point.PointStatus.Mouse;

            }
            //empty old position
            this.Grid[Mouse.Position.X, Mouse.Position.Y].Status = Point.PointStatus.Empty;
            this.Mouse.Position = this.Grid[nextX, nextY];
            return false;
        }

        /// <summary>
        /// Places the cheese on a random point on the grid
        /// </summary>
        public void PlaceCheese()
        {
            //randomize the points for the cheese on the grid
            do
            {
                this.Cheese = Grid[rng.Next(0, 10), rng.Next(0, 10)];
            } while (this.Cheese.Status != Point.PointStatus.Empty);

            //adds cheese to grid with random coordinates
            this.Cheese.Status = Point.PointStatus.Cheese;
        }

        /// <summary>
        /// Adds a new cat to the Cats list
        /// </summary>
        public void AddCat()
        {
            //create the cat
            Cat cat = new Cat();
            PlaceCat(cat);//calls PlaceCat with new cat
            this.Cats.Add(cat);//adds new cat to the Cats list
        }

        /// <summary>
        /// Places the cat on the grid
        /// </summary>
        /// <param name="cat">Cat from the cat list</param>
        public void PlaceCat(Cat cat)
        {
            //randomly picks points on the grid to place cat
            do{
                cat.Position = this.Grid[rng.Next(this.Grid.GetLength(0)), rng.Next(this.Grid.GetLength(1))];
                
            } while(cat.Position.Status != Point.PointStatus.Empty);
            //sets cat position to the random coordinates
            cat.Position.Status = Point.PointStatus.Cat;
        }

        /// <summary>
        /// automatically moves the cat on the grid
        /// </summary>
        /// <param name="cat">Cat to be moved</param>
        public void MoveCat(Cat cat)
        {
            //80% chance of cat moving
            if (rng.Next(0, 6) < 4)
            {
                //booleans for moving cat
                int xDiff = Mouse.Position.X - cat.Position.X;
                int yDiff = Mouse.Position.Y - cat.Position.Y;
                bool tryLeft = xDiff < 0;
                bool tryRight = xDiff > 0;
                bool tryUp = yDiff < 0;
                bool tryDown = yDiff > 0;
                bool validMove = false;
                //cats future position on grid
                Point targetPosition = cat.Position;

                //checks if move is valid
                while (!validMove && (tryLeft || tryRight || tryUp || tryDown))
                {
                    //new cat coordinates
                    int catX = cat.Position.X;
                    int catY = cat.Position.Y;
                    
                    if (tryLeft)//if room to move left
                    {
                        targetPosition = Grid[--catX, catY];//moves cat left
                        tryLeft = false;
                    }
                    else if (tryRight)//if room to move right
                    {
                        targetPosition = Grid[++catX, catY];//moves cat right
                        tryRight = false;
                    }
                    else if (tryUp)//if room to move up
                    {
                        targetPosition = Grid[catX, --catY];//moves cat up
                        tryUp = false;
                    }
                    else if (tryDown)//if room to move down
                    {
                        targetPosition = Grid[catX, ++catY];//moves cat down
                        tryDown = false;
                    }
                    //calls valid cat function with targetposition as parameter
                    validMove = ValidCatMoveOnBoard(targetPosition);

                }

                // Look at the next move and change the status
                switch (targetPosition.Status)
                {
                    case Point.PointStatus.Cheese:
                        targetPosition.Status = Point.PointStatus.CatAndCheese;
                        break;
                    case Point.PointStatus.Mouse:
                        this.Mouse.HasBeenPouncedOn = true;
                        targetPosition.Status = Point.PointStatus.Cat;
                        break;
                    default:
                        targetPosition.Status = Point.PointStatus.Cat;
                        break;
                }

                // handle previous position status before executing move
                if (cat.Position.Status == Point.PointStatus.CatAndCheese)
                {
                    cat.Position.Status = Point.PointStatus.Cheese;
                }
                else
                {
                    cat.Position.Status = Point.PointStatus.Empty;
                }
                // execute the move
                cat.Position = targetPosition;
            }

        }
        
        /// <summary>
        /// Checks if the cat's move is valid
        /// </summary>
        /// <param name="targetLocation">Point on the grid the cat is moving towards</param>
        /// <returns>True or false</returns>
        public bool ValidCatMoveOnBoard(Point targetLocation)
        {
            //checks if the future cat move is to any spot other than one already occupied by a cat
            return targetLocation.Status == Point.PointStatus.Empty || targetLocation.Status == Point.PointStatus.Mouse || targetLocation.Status == Point.PointStatus.Cheese;
        }

        /// <summary>
        /// Automatically moves the cat on the board
        /// </summary>
        /// <param name="targetPosition">Position to move the cat</param>
        /// <returns>true or false</returns>
        public bool CatsTurnToMove(Point targetPosition)
        {
            //checks position if the position has a cat
            return targetPosition.Status == Point.PointStatus.Cat || targetPosition.Status == Point.PointStatus.CatAndCheese;
        }

        /// <summary>
        /// Plays the game 
        /// </summary>
        public void PlayGame()
        {
            Mouse.HasBeenPouncedOn = false;
            //boolean finding cheese
            bool hasFoundCheese = false;
            //cheese counter
            this.CheeseCount = 0;

            //game play condition
            while(Mouse.Energy > 0 && !Mouse.HasBeenPouncedOn)
            {
                //Draw the grid
                DrawGrid();
                //Get valid user input
                ConsoleKey playerMove = GetUserMove();

                //move the mouse and determine
                //if the game has ended
                hasFoundCheese = MoveMouse(playerMove);

                //loops through the cats in cat list
                foreach(Cat cat in Cats)
                {
                    //run each cat through move cat list
                    this.MoveCat(cat);
                }
                //if cheese was found
                if (hasFoundCheese)
                {
                    //replaces the cheese
                    PlaceCheese();
                    //adds to the mouse's energy
                    this.Mouse.Energy += 10;
                    //add to the cheese counter
                    CheeseCount++;
                    //for every two pieces of cheese eaten
                    if (CheeseCount % 2 == 0)
                    {
                        //add a cat to the grid
                        AddCat();
                    }
                }
            }
            // Update last status on death
            DrawGrid();
            // determine cause of death
            if(this.Mouse.Energy == 0)
            {
                Console.WriteLine("You ran out of energy and died");
            }
            else
            {
                Console.WriteLine("You were eaten by the cat!");
            }
        }
    }
}
